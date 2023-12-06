using System;
using ProducerService.API.Models.Events;

namespace ProducerService.API.Services
{
	public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}

