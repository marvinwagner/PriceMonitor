using Microsoft.EntityFrameworkCore;
using PriceMonitor.Core.Data;
using PriceMonitor.WebApi.Data;
using PriceMonitor.WebApi.Data.Repositories;
using PriceMonitor.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnyMal.Clients.Api.Data.Repositories
{
    public sealed class ItemRepository : IItemRepository
    {
        private readonly MonitorContext _context;

        public ItemRepository(MonitorContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;


        public async Task<IEnumerable<Item>> ListAll()
        {
            return await _context.Items.AsNoTracking().ToListAsync();
        }

        public async Task<Item> FindById(Guid id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> FindByUrl(string url)
        {
            return await _context.Items.AsNoTracking().FirstOrDefaultAsync(x => x.Url == url);
        }

        public void Create(Item item)
        {
            _context.Items.Add(item);
        }

        public void Update(Item item)
        {
            _context.Items.Update(item);
        }

        public void Delete(Item item)
        {
            _context.Items.Remove(item);
        }

        public void AddHistory(ItemHistory history)
        {
            _context.ItemHistories.Add(history);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
