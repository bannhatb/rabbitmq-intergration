using System;
namespace ConsumerService.API.Services
{
	public interface IEventBusService
	{
		void Publish(object message);

	}
}

