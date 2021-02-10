using PriceMonitor.Core.Domain;
using System;

namespace PriceMonitor.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
