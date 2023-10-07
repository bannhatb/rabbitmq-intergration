using System;
using RabbitMQ.Client;

namespace ProducerService.API.Services
{
	public interface IRabbitMQPersistentConnection : IDisposable
	{
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}

