using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents
{
    public record CinemaHallCreatedEvent(Guid HallId, ScreeningTechnology hallTechnology, short SeatCount) : IDomainEvent
    {
    }
}
