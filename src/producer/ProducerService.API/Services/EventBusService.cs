﻿using System;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;

namespace ProducerService.API.Services
{
    public class EventBusService : IEventBusService
    {
        const string BROKER_NAME = "my_event_bus";

        private string? _queueName;

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusService> _logger;

        public EventBusService(IRabbitMQPersistentConnection persistentConnection, ILogger<EventBusService> logger, IOptions<EventBusSettings> options)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _queueName = options.Value.SubscriptionClientName;
        }

        public void Publish(object message)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = message.GetType().Name;

                channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

                var body = JsonSerializer.SerializeToUtf8Bytes(message, message.GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                _logger.LogTrace("Publishing event to RabbitMQ");

                channel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            }
        }
    }
}
