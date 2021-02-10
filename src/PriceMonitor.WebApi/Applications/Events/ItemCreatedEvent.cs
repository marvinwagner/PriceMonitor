using PriceMonitor.Core.Messages;
using System;

namespace PriceMonitor.WebApi.Applications.Events
{
    public class ItemCreatedEvent : Event
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }

        public ItemCreatedEvent(Guid id, string name, string url)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Url = url;
        }
    }
}