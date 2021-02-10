using PriceMonitor.Core.Domain;
using System;

namespace PriceMonitor.WebApi.Models
{
    public class ItemHistory : Entity<ItemHistory>
    {
        public Guid ItemId { get; private set; }
        public decimal InCashValue { get; private set; }
        public decimal NormalValue { get; private set; }
        public decimal FullValue { get; private set; }
        public bool Available { get; private set; }
        // EF Core
        public Item Item { get; private set; }

        public ItemHistory(Guid itemId, decimal inCashValue, decimal normalValue, decimal fullValue, bool available)
        {
            ItemId = itemId;
            InCashValue = inCashValue;
            NormalValue = normalValue;
            FullValue = fullValue;
            Available = available;
        }
    }
}
