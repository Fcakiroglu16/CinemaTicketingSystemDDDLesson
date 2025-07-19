using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents;
using MediatR;

namespace CinemaTicketingSystem.Application.CinemaManagement.Cinema.EventHandlers
{
    internal class CinemaHallCreatedEventHandler(IIntegrationEventBus integrationEventBus) : INotificationHandler<CinemaHallCreatedEvent>
    {
        public async Task Handle(CinemaHallCreatedEvent notification, CancellationToken cancellationToken)
        {

            Console.WriteLine("CinemaHallCreatedEventHandler çalıştı");

            await integrationEventBus.PublishAsync(notification, cancellationToken);


        }
    }
}
