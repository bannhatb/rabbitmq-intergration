using System;
using System.Text;
using ConsumerService.API.Models;
using ConsumerService.API.Services.EventHandlers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProducerService.API.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerService.API.Services
{
	public class RabbitConsumerService : IHostedService, IDisposable
    {
        const string BROKER_NAME = "my_event_bus";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<RabbitConsumerService> _logger;

        private IModel? _consumerChannel;
        private string? _queueName;
        private List<string> _eventNames;

        public RabbitConsumerService(IRabbitMQPersistentConnection persistentConnection,
                                     ILogger<RabbitConsumerService> logger,
                                     IOptions<EventBusSettings> options,
                                     List<string> eventNames)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _queueName = options.Value.SubscriptionClientName;
            _eventNames = eventNames;
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _consumerChannel = CreateConsumerChannel();

            // subscribe event
            foreach (var eventName in _eventNames)
            {
                DoInternalSubscription(eventName);
            }
            //
            StartBasicConsume();
            await Task.CompletedTask;

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            await Task.CompletedTask;
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel?.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                _logger.LogTrace(message);
                // handle event
                switch (eventName)
                {
                    case nameof(MessageModel):
                        var obj = JsonConvert.DeserializeObject<MessageModel>(message);
                        // call handler
                        var handler = new SampleHandler();
                        handler.Process(obj);
                        break;
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private void DoInternalSubscription(string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _consumerChannel.QueueBind(queue: _queueName,
                                exchange: BROKER_NAME,
                                routingKey: eventName);
        }
    }
}

