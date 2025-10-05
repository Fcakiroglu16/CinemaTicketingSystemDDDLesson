#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;
using Microsoft.EntityFrameworkCore.Diagnostics;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Interceptors;

internal class DomainEventsInterceptor(
    IIntegrationEventBus? integrationEventBus,
    IDomainEventMediator? domainEventMediator) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (domainEventMediator is null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

        if (eventData.Context is null) return await base.SavingChangesAsync(eventData, result, cancellationToken);


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

        foreach (var ev in events) await domainEventMediator.PublishAsync(ev, cancellationToken);


        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new())
    {
        if (integrationEventBus is null) return await base.SavedChangesAsync(eventData, result, cancellationToken);


        if (eventData.Context is null) return await base.SavedChangesAsync(eventData, result, cancellationToken);

        var aggregates = eventData.Context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.IntegrationEvents.Any())
            .Select(e => e.Entity)
            .ToList();


        var integrationEvents = new List<IIntegrationEvent>();
        foreach (var aggr in aggregates)
        {
            integrationEvents.AddRange(aggr.IntegrationEvents);
            aggr.ClearDomainEvents();
        }

        foreach (var ev in integrationEvents) await integrationEventBus.PublishAsync(ev, cancellationToken);


        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}