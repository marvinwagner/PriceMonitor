using PriceMonitor.WebApi.Models;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PriceMonitor.WebApi.Services.Scrapping
{
    public interface IExtractor
    {
        public decimal InCashValue { get; set; }
        public decimal NormalValue { get; set; }
        public decimal FullValue { get; set; }
        public bool IsAvailable { get; set; }

        Task<bool> ExtractValues(Item item, CancellationToken cancellationToken);
    }

    public static class ExtractorFactory
    {
        public static IExtractor Create(string url)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().First(x => x.GetCustomAttributes().Any(attr => attr is WebsiteAttribute && url.Contains((attr as WebsiteAttribute).Name)));
            var extractor = Activator.CreateInstance(type);
            return extractor as IExtractor;
        }
    }
}

