namespace CinemaTicketingSystem.Domain.Ticketing.DomainEvents;

public record TicketReleasedEvent(Guid TicketId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}