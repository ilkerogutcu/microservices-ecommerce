using System;
using Newtonsoft.Json;

namespace EventBus.Base.Events
{
    public class IntegrationEvent
    {
        [JsonProperty] public Guid Id { get; }
        [JsonProperty] public DateTime CreatedDate { get; }


        public IntegrationEvent()
        {
            Id = new Guid();
            CreatedDate = DateTime.Now;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createdDate)
        {
            Id = id;
            CreatedDate = createdDate;
        }
    }
}