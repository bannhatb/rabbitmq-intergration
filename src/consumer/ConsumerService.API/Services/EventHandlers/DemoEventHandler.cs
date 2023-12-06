using System;
using ConsumerService.API.Models.Events;

namespace ConsumerService.API.Services.EventHandlers
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

