using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Order.Domain.SeedWork;
using Order.Infrastructure.Context;

namespace Order.Infrastructure.Extensions
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, OrderDbContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList().ForEach(x => x.Entity.ClearDomainEvents());

            foreach (var @event in domainEvents)
                await mediator.Publish(@event);
        }
    }
}