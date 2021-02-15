using Microsoft.AspNetCore.SignalR;
using PriceMonitor.WebApi.Applications.Events;
using System.Threading.Tasks;

namespace PriceMonitor.WebApi.SignalR
{
    public interface IItemEventsHub
    {
        Task PriceUpdated(PriceUpdatedEvent evt);
    }
    public class ItemEventsHub : Hub<IItemEventsHub>
    {
    }
}
