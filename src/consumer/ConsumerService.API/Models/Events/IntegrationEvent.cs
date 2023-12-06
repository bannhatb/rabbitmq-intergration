using System;
using System.Text.Json.Serialization;

namespace ConsumerService.API.Models.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTimeOffset.Now;
        }

        [JsonInclude]
        public Guid Id { get; private init; }

        [JsonInclude]
        public DateTimeOffset CreationDate { get; private init; }
    }
}

