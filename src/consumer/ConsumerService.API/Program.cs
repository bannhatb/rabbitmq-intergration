using ConsumerService.API;
using ConsumerService.API.Models.Entities;
using ConsumerService.API.Models.Events;
using ConsumerService.API.Repositories;
using ConsumerService.API.Services;
using ConsumerService.API.Services.EventHandlers;
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

// register handler
builder.Services.AddScoped<IIntegrationEventHandler<DemoEvent>, DemoEventHandler>();
builder.Services.AddScoped<IIntegrationEventHandler<TestResultEvent>, TestResultEventHandler>();
builder.Services.AddSingleton<ISubscriptionManager>(x =>
{
    var subscription = new SubscriptionManager();
    subscription.AddSubscription<DemoEvent, IIntegrationEventHandler<DemoEvent>>();
    // more event
    subscription.AddSubscription<TestResultEvent, IIntegrationEventHandler<TestResultEvent>>();
    return subscription;
});
// Service chay ngam
builder.Services.AddHostedService(sp =>
{
    var connection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var logger = sp.GetRequiredService<ILogger<RabbitConsumerService>>();
    var configOptions = sp.GetRequiredService<IOptions<EventBusSettings>>();
    var subscriptionManager = sp.GetRequiredService<ISubscriptionManager>();

    return new RabbitConsumerService(connection, logger, configOptions, sp, subscriptionManager);
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

