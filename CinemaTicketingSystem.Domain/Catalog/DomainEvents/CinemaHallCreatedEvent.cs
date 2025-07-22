using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Catalog.DomainEvents;

public record CinemaHallCreatedEvent(Guid HallId, ScreeningTechnology hallTechnology, short SeatCount) : IDomainEvent
{
}