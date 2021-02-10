using PriceMonitor.Core.Domain;
using System.Collections;
using System.Collections.Generic;

namespace PriceMonitor.WebApi.Models
{
    public class Item : Entity<Item>, IAggregateRoot
    {
        private readonly List<ItemHistory> _history;
        public string Name { get; private set; }
        public string Url { get; private set; }
        public decimal CurrentInCashValue { get; private set; }
        public decimal CurrentNormalValue { get; private set; }
        public decimal CurrentFullValue { get; private set; }
        public bool Available { get; private set; }
        // EF Core
        public IReadOnlyCollection<ItemHistory> History => _history;

        protected Item() { }

        public Item(string name, string url)
        {
            Name = name;
            Url = url;
            Available = false;
            _history = new List<ItemHistory>();
        }

        public void UpdateValues(decimal inCash, decimal normal, decimal full)
        {
            CurrentInCashValue = inCash;
            CurrentNormalValue = normal;
            CurrentFullValue = full;
        }

        public void SetAvailability(bool available)
        {
            Available = available;
        }
    }
}
