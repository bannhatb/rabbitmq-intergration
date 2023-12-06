using System;
using ProducerService.API.Models.Events;

namespace ProducerService.API.Services
{
	public interface ISubscriptionManager
	{

        bool IsEmpty { get; }

        void AddSubscription<T, TH>()
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>;

        string GetEventKey<T>();

        bool HasSubscriptionsForEvent(string eventName);

        IEnumerable<string> GetEvents();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        Type? GetEventTypeByName(string eventName);
    }
}

