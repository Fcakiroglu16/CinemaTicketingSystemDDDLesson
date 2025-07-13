namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record ReservationCreatedEvent(Guid ReservationId, Guid CustomerId, Guid MovieSessionId, DateTime ReservationTime)
    : IDomainEvent
{

    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}