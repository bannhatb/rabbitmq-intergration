using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProducerService.API.Services
{
    public class RabbitProducerService : IHostedService, IDisposable
    {
        const string BROKER_NAME = "my_event_bus";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<RabbitProducerService> _logger;
        private readonly ISubscriptionManager _subscriptionManager;
        private readonly IServiceProvider _serviceProvider;
        private IModel? _producerChannel;
        private string? _queueName;

        public RabbitProducerService(IRabbitMQPersistentConnection persistentConnection,
                                     ILogger<RabbitProducerService> logger,
                                     IOptions<EventBusSettings> options,
                                     IServiceProvider serviceProvider,
                                     ISubscriptionManager subscriptionManager)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _queueName = options.Value.SubscriptionClientName;
            _subscriptionManager = subscriptionManager;
            _serviceProvider = serviceProvider;
        }

        public void Dispose()
        {
            if (_producerChannel != null)
            {
                _producerChannel.Dispose();
            }

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _producerChannel = CreateProducerChannel();

            // subscribe event
            var events = _subscriptionManager.GetEvents();
            foreach (var eventName in events)
            {
                DoInternalSubscription(eventName);
            }
            //
            StartBasicProducer();
            await Task.CompletedTask;

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_producerChannel != null)
            {
                _producerChannel.Dispose();
            }

            await Task.CompletedTask;
        }

        private IModel CreateProducerChannel()
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

                _producerChannel?.Dispose();
                _producerChannel = CreateProducerChannel();
                StartBasicProducer();
            };

            return channel;
        }

        private void StartBasicProducer()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_producerChannel != null)
            {
                var producer = new AsyncEventingBasicConsumer(_producerChannel);

                producer.Received += Producer_Received;

                _producerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: producer);
            }
            else
            {
                _logger.LogError("StartBasicProducer can't call on _producerChannel == null");
            }
        }

        private async Task Producer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _producerChannel?.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetRequiredService(subscription.HandlerType);

                        if (handler == null)
                        {
                            continue;
                        }

                        var eventType = _subscriptionManager.GetEventTypeByName(eventName);

                        if (eventType == null)
                        {
                            continue;
                        }

                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        if (integrationEvent == null)
                        {
                            continue;
                        }

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        if (concreteType == null)
                        {
                            continue;
                        }

                        await Task.Yield();

                        await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, new object[] { integrationEvent })!;
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
        private void DoInternalSubscription(string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _producerChannel.QueueBind(queue: _queueName,
                                exchange: BROKER_NAME,
                                routingKey: eventName);
        }
    }
}

