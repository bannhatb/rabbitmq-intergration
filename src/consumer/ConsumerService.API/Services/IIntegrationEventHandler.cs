using System;
using ConsumerService.API.Models.Events;

namespace ConsumerService.API.Services
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}

