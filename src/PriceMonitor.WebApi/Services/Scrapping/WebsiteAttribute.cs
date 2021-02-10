using System;

namespace PriceMonitor.WebApi.Services.Scrapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WebsiteAttribute : Attribute
    {
        public string Name { get; private set; }

        public WebsiteAttribute(string name)
        {
            this.Name = name;
        }
    }
}
