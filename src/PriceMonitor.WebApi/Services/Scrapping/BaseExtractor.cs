using AngleSharp;
using AngleSharp.Dom;
using PriceMonitor.WebApi.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PriceMonitor.WebApi.Services.Scrapping
{
    public abstract class BaseExtractor
    {
        protected const string VALUE_REGEXP = @"(?!R\$\s?)[1-9]\d{0,2}(\.\d{3})*,\d{2}";

        protected IDocument Document { get; set; }

        public decimal InCashValue { get; set; }
        public decimal NormalValue { get; set; }
        public decimal FullValue { get; set; }
        public bool IsAvailable { get; set; }

        public virtual async Task Call(string url, CancellationToken ct = default)
        {
            // Load default configuration
            var config = Configuration.Default.WithDefaultLoader();
            // Create a new browsing context
            var context = BrowsingContext.New(config);

            Document = await context.OpenAsync(url, ct);
        }

        protected abstract bool ConfirmIfLoaded();
        protected abstract void FillValues();
        protected abstract void CheckAvailability();

        public async Task<bool> ExtractValues(Item item, CancellationToken cancellationToken = default)
        {
            await Call(item.Url, cancellationToken);

            if (!ConfirmIfLoaded()) return false;

            FillValues();
            CheckAvailability();

            return InCashValue != item.CurrentInCashValue ||
                   NormalValue != item.CurrentNormalValue ||
                   FullValue != item.CurrentFullValue ||
                   IsAvailable != item.IsAvailable;
        }


        protected decimal ExtractValueFromTags(string[] tags)
        {
            IElement element = null;
            foreach (var tag in tags)
            {
                element = Document.QuerySelector(tag);
                if (element != null)
                    break;
            }

            if (element != null)
            {
                return Convert.ToDecimal(Regex.Match(element.InnerHtml.Trim(), VALUE_REGEXP).Value, CultureInfo.GetCultureInfo("pt-BR"));
            }
            return -1;
        }
    }
}