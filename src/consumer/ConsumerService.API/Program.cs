using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using ConsumerService.API;
using ConsumerService.API.Models.Entities;
using ConsumerService.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
    var logger = sp.GetRequiredService<ILogger<RabbitConsumerService>>();
    var configOptions = sp.GetRequiredService<IOptions<EventBusSettings>>();

    // list subscribe
    var eventNames = new List<string>()
    {
        nameof(MessageModel)
    };

    return new RabbitConsumerService(connection, logger, configOptions, eventNames);
});

//

builder.Services.AddControllers();

//Connection SQL server
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "PBL6.WebAPI", Version = "v1" });
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

        //// using System.Reflection;
        //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

//var audience = builder.Configuration.GetSection("Audience").Get<Audience>();

//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

//var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audience.Secret));
//var tokenValidationParameters = new TokenValidationParameters //verify token
//{
//    ValidateIssuerSigningKey = true,
//    IssuerSigningKey = signingKey,
//    ValidateIssuer = true,
//    ValidIssuer = audience.Issuer,
//    ValidateAudience = true,
//    ValidAudience = audience.Name,
//    ValidateLifetime = true,
//    ClockSkew = TimeSpan.Zero,
//    RequireExpirationTime = true,
//};
//builder.Services.AddAuthentication()
//    .AddJwtBearer("MyAuthKey", options =>
//    {
//        options.RequireHttpsMetadata = false;
//        options.SaveToken = true;
//        options.TokenValidationParameters = tokenValidationParameters;
//    });

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

