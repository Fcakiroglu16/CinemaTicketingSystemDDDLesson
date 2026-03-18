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


        List<IAggregateRoot> aggregates = eventData.Context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        List<IDomainEvent> events = new List<IDomainEvent>();
        foreach (IAggregateRoot aggr in aggregates)
        {
            events.AddRange(aggr.DomainEvents);
            aggr.ClearDomainEvents();
        }

        foreach (IDomainEvent ev in events) await domainEventMediator.PublishAsync(ev, cancellationToken);


        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new())
    {
        if (integrationEventBus is null) return await base.SavedChangesAsync(eventData, result, cancellationToken);


        if (eventData.Context is null) return await base.SavedChangesAsync(eventData, result, cancellationToken);

        List<IAggregateRoot> aggregates = eventData.Context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.IntegrationEvents.Any())
            .Select(e => e.Entity)
            .ToList();


        List<IIntegrationEvent> integrationEvents = new List<IIntegrationEvent>();
        foreach (IAggregateRoot aggr in aggregates)
        {
            integrationEvents.AddRange(aggr.IntegrationEvents);
            aggr.ClearIntegrationEvents();
        }

        foreach (IIntegrationEvent ev in integrationEvents) await integrationEventBus.PublishAsync(ev, cancellationToken);


        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}