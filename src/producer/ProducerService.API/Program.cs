
using ProducerService.API;
using ProducerService.API.Models.Entities;
using ProducerService.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ProducerService.API.DTOs;
using ProducerService.API.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ProducerService.API.Services.EventHandlers;
using ProducerService.API.Models.Events;

var builder = WebApplication.CreateBuilder(args);
// Connect postgres database
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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITestUserRepository, TestUserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// register handler
builder.Services.AddScoped<IIntegrationEventHandler<ScoreUserTestEvent>, TestUserHandler>();

builder.Services.AddSingleton<ISubscriptionManager>(x =>
{
    var subscription = new SubscriptionManager();
    // more event
    subscription.AddSubscription<ScoreUserTestEvent, IIntegrationEventHandler<ScoreUserTestEvent>>();
    return subscription;
});
// Service chay ngam
builder.Services.AddHostedService(sp =>
{
    var connection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var logger = sp.GetRequiredService<ILogger<RabbitProducerService>>();
    var configOptions = sp.GetRequiredService<IOptions<EventBusSettings>>();
    var subscriptionManager = sp.GetRequiredService<ISubscriptionManager>();

    return new RabbitProducerService(connection, logger, configOptions, sp, subscriptionManager);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials(); // allow credentials
        });
});

builder.Services.Configure<Audience>(builder.Configuration.GetSection("Audience"));
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PBL7.TracNghiemRabbitMQ-ProducerService.API", Version = "v1" });
    //add frame to get token bearer
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' following by space and JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "MyAuthKey"
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    // // using System.Reflection;
    // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var audience = builder.Configuration.GetSection("Audience").Get<Audience>();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audience.Secret));
var tokenValidationParameters = new TokenValidationParameters //verify token
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateIssuer = true,
    ValidIssuer = audience.Issuer,
    ValidateAudience = true,
    ValidAudience = audience.Name,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    RequireExpirationTime = true,
};

builder.Services.AddAuthentication()
    .AddJwtBearer("MyAuthKey", options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = tokenValidationParameters;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

