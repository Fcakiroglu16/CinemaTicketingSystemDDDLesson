using CinemaTicketingSystem.Domain.Ticketing.Tickets.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record SeatReservationReleasedEvent(Guid ReservationId, SeatNumber SeatNumber) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}