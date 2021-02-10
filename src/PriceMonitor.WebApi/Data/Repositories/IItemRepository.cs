using PriceMonitor.Core.Data;
using PriceMonitor.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceMonitor.WebApi.Data.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> ListAll();
        Task<Item> FindById(Guid id);
        Task<Item> FindByUrl(string url);
        
        void Create(Item item);
        void Update(Item item);
        void Delete(Item item);

        void AddHistory(ItemHistory history);
    }
}
