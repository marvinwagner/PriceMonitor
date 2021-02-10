using PriceMonitor.Core.Messages;
using System.Collections.Generic;

namespace PriceMonitor.Core.Domain
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<Event> Notifications { get; }
        void ClearEvents();
    }
}