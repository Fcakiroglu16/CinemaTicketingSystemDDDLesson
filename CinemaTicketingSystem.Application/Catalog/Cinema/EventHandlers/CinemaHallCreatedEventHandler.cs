using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.Catalog.DomainEvents;
using MediatR;

namespace CinemaTicketingSystem.Application.Catalog.Cinema.EventHandlers;

internal class CinemaHallCreatedEventHandler(IIntegrationEventBus integrationEventBus)
    : INotificationHandler<CinemaHallCreatedEvent>
{
    public async Task Handle(CinemaHallCreatedEvent notification, CancellationToken cancellationToken)
    {


        await integrationEventBus.PublishAsync(notification, cancellationToken);
    }
}