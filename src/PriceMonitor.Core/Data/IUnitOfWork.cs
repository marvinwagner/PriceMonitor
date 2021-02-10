using System.Threading.Tasks;

namespace PriceMonitor.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
