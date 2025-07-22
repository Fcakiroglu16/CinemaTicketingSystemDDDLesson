using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Domain.Catalog.DomainEvents;
using MediatR;

namespace CinemaTicketingSystem.Application.Catalog.Movie.EventHandlers;

internal class MovieCreatedEventHandler(IIntegrationEventBus integrationEventBus)
    : INotificationHandler<MovieCreatedEvent>
{
    public Task Handle(MovieCreatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}