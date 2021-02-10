using AnyMal.Clients.Api.Data.Repositories;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PriceMonitor.WebApi.Applications.Commands;
using PriceMonitor.WebApi.Applications.Events;
using PriceMonitor.WebApi.Data;
using PriceMonitor.WebApi.Data.Repositories;

namespace PriceMonitor.WebApi.Setup
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<IRequestHandler<CreateItemCommand, ValidationResult>, ItemCommandHandler>();
            services.AddScoped<IRequestHandler<UpdatePriceCommand, ValidationResult>, ItemCommandHandler>();

            services.AddScoped<INotificationHandler<ItemCreatedEvent>, ItemEventHandler>();
            services.AddScoped<INotificationHandler<PriceUpdatedEvent>, ItemEventHandler>();

            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<MonitorContext>();
        }
    }
}
