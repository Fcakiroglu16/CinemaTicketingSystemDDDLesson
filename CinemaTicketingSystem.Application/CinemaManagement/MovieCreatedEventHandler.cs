using CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents;
using MediatR;

namespace CinemaTicketingSystem.Application.CinemaManagement
{
    internal class MovieCreatedEventHandler : INotificationHandler<MovieCreatedEvent>
    {
        public Task Handle(MovieCreatedEvent notification, CancellationToken cancellationToken)
        {
            Console.Write(notification.MovieId);

            return Task.CompletedTask;
        }
    }
}
