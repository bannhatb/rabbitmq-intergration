using System;
using RabbitMQ.Client;

namespace ConsumerService.API.Services
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}

