using MediatR;
using Microsoft.AspNetCore.SignalR;
using PriceMonitor.WebApi.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace PriceMonitor.WebApi.Applications.Events
{
    public class ItemEventHandler : 
        INotificationHandler<ItemCreatedEvent>,
        INotificationHandler<PriceUpdatedEvent>
    {
        private readonly IHubContext<ItemEventsHub, IItemEventsHub> _hubContext;

        public ItemEventHandler(IHubContext<ItemEventsHub, IItemEventsHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public Task Handle(ItemCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PriceUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _hubContext.Clients.All.PriceUpdated(notification);

            return Task.CompletedTask;
        }
    }
}