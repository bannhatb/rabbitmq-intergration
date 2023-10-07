using System;
namespace ProducerService.API.Services
{
	public interface IEventBusService
	{
		void Publish(object message);

    }
}

