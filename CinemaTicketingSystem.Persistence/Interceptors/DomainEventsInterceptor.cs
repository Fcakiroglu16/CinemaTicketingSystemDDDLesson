using CinemaTicketingSystem.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CinemaTicketingSystem.Persistence.Interceptors;

internal class DomainEventsInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null) return await base.SavedChangesAsync(eventData, result, cancellationToken);


        var aggregates = eventData.Context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = new List<IDomainEvent>();
        foreach (var aggr in aggregates)
        {
            events.AddRange(aggr.DomainEvents);
            aggr.ClearDomainEvents();
        }

        foreach (var ev in events) await publisher.Publish(ev, cancellationToken);


        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}