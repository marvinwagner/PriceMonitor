using System;

namespace PriceMonitor.Core.Messages
{
    public abstract class Message
    {
        public DateTime Timespamp { get; private set; }
        public string MessageType { get; private set; }
        public Guid AggregateId { get; set; }

        protected Message()
        {
            MessageType = GetType().Name;
            Timespamp = DateTime.Now;
        }
    }
}
