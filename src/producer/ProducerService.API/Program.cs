
using ProducerService.API;
using ProducerService.API.Models.Entities;
using ProducerService.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");

// Add services to the container.
builder.Services.AddOptions<EventBusSettings>().BindConfiguration("EventBusSettings");

// register rabbitmq
var eventBusSettings = new EventBusSettings();
builder.Configuration.GetSection("EventBusSettings").Bind(eventBusSettings);
builder.Services.AddSingleton<IConnectionFactory>(options =>
{
    var factory = new ConnectionFactory()
    {
        HostName = eventBusSettings.EventBusConnection,
        UserName = eventBusSettings.EventBusUserName,
        Password = eventBusSettings.EventBusPassword,
        DispatchConsumersAsync = true
    };

    return factory;
});
builder.Services.AddSingleton<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();

builder.Services.AddScoped<IEventBusService, EventBusService>();

// Service chay ngam
builder.Services.AddHostedService(sp =>
{
    var connection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var logger = sp.GetRequiredService<ILogger<RabbitProducerService>>();
    var configOptions = sp.GetRequiredService<IOptions<EventBusSettings>>();

    // list subscribe
    var eventNames = new List<string>()
    {
        nameof(MessageModel)
    };

    return new RabbitProducerService(connection, logger, configOptions, eventNames);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

