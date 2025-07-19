using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents;
using MediatR;

namespace CinemaTicketingSystem.Application.CinemaManagement.Movie.EventHandlers
{
    internal class MovieCreatedEventHandler(IIntegrationEventBus integrationEventBus) : INotificationHandler<MovieCreatedEvent>
    {
        public async Task Handle(MovieCreatedEvent notification, CancellationToken cancellationToken)
        {

            await integrationEventBus.PublishAsync(notification, cancellationToken);
        }
    }
}
