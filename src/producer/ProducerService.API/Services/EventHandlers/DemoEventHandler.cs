using System;
using ProducerService.API.Models.Events;

namespace ProducerService.API.Services.EventHandlers
{
	public class DemoEventHandler : IIntegrationEventHandler<DemoEvent>
    {
        // inject repo, any service
		public DemoEventHandler()
		{
		}


        public Task Handle(DemoEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}

