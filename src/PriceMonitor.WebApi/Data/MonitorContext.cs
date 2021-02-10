using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PriceMonitor.Core.Data;
using PriceMonitor.Core.Domain;
using PriceMonitor.Core.Messages;
using PriceMonitor.WebApi.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitor.WebApi.Data
{
    public class MonitorContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public MonitorContext(DbContextOptions<MonitorContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemHistory> ItemHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MonitorContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            var success = await base.SaveChangesAsync() > 0;
            if (success) await _mediator.PublishEvents(this);

            return success;
        }
    }

    internal static class MediatrExtensions
    {
        public static async Task PublishEvents(this IMediator mediator, MonitorContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<IAggregateRoot>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(e => e.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async domainEvent =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
