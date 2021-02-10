using PriceMonitor.Core.Messages;
using System;

namespace PriceMonitor.WebApi.Applications.Events
{
    public class PriceUpdatedEvent : Event
    {
        public Guid ItemId { get; private set; }
        public decimal InCashValue { get; private set; }
        public decimal NormalValue { get; private set; }
        public decimal FullValue { get; private set; }
        public bool Available { get; private set; }

        public PriceUpdatedEvent(Guid itemId, decimal inCashValue, decimal normalValue, decimal fullValue, bool available)
        {
            AggregateId = itemId;
            ItemId = itemId;
            InCashValue = inCashValue;
            NormalValue = normalValue;
            FullValue = fullValue;
            Available = available;
        }
    }
}
