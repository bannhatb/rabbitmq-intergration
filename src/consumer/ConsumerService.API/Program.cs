using ConsumerService.API;
using ConsumerService.API.DTOs;
using ConsumerService.API.Models.Entities;
using ConsumerService.API.Repositories;
using ConsumerService.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
// Connect MongoDB
var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

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
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

// Service chay ngam
builder.Services.AddHostedService(sp =>
{
    var connection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var logger = sp.GetRequiredService<ILogger<RabbitConsumerService>>();
    var configOptions = sp.GetRequiredService<IOptions<EventBusSettings>>();

    // list subscribe
    var eventNames = new List<string>()
    {
        nameof(MessageModel),
        nameof(TestResultDto)
    };

    return new RabbitConsumerService(connection, logger, configOptions, eventNames);
});

//

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

