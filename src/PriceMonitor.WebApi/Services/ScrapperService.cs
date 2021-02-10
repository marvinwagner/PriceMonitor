using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PriceMonitor.WebApi.Applications.Commands;
using PriceMonitor.WebApi.Data.Repositories;
using PriceMonitor.WebApi.Services.Scrapping;

namespace PriceMonitor.WebApi.Services
{
    public sealed class ScrapperService : IHostedService, IDisposable
    {
        private readonly ILogger<ScrapperService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public ScrapperService(ILogger<ScrapperService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scrapper Hosted Service running.");


            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }


        private async void DoWork(object state)
        {
            _logger.LogInformation("Scrapper Hosted Service is working.");

            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var commandList = new ConcurrentBag<UpdatePriceCommand>();

            var items = await repository.ListAll();
            var tasks = items.Select(async (x) =>
            {
                try
                {
                    var extractor = ExtractorFactory.Create(x.Url);
                    if (await extractor.ExtractValues(x))
                    {
                        _logger.LogInformation($"Price changed for {x.Name}");
                        commandList.Add(new UpdatePriceCommand(x.Id, extractor.InCashValue, extractor.NormalValue, extractor.FullValue, extractor.IsAvailable));
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error calling url for product {x.Name}.", e);
                }
            });

            await Task.WhenAll(tasks);

            foreach (var cmd in commandList)
            {
                await mediator.Send(cmd);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scrapper Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}