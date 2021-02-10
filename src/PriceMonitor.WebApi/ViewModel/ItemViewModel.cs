using System;

namespace PriceMonitor.WebApi.ViewModel
{
    public class ItemViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class ResultItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public decimal CurrentInCashValue { get; set; }
        public decimal CurrentNormalValue { get; set; }
        public decimal CurrentFullValue { get; set; }
        public bool IsAvailable { get; set; }
    }
}
