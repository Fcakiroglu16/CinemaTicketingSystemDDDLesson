namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record ReservationConfirmedEvent(Guid ReservationId, Guid CustomerId, Guid MovieSessionId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}