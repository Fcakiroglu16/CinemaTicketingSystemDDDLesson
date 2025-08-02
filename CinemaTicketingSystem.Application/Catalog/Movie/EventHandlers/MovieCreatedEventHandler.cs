using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;
using MediatR;

namespace CinemaTicketingSystem.Application.Catalog.Movie.EventHandlers;

internal class MovieCreatedEventHandler(IIntegrationEventBus integrationEventBus)
    : INotificationHandler<MovieCreatedEvent>
{
    public async Task Handle(MovieCreatedEvent notification, CancellationToken cancellationToken)
    {
        await integrationEventBus.PublishAsync(notification, cancellationToken);
    }
}