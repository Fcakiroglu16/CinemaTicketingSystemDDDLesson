using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;

public record CinemaHallCreatedEvent(Guid HallId, ScreeningTechnology hallTechnology, short SeatCount) : IDomainEvent
{
}