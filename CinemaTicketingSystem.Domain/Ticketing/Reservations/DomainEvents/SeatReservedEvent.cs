using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record SeatReservedEvent(Guid ReservationId, SeatNumber SeatNumber, Guid CustomerId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}