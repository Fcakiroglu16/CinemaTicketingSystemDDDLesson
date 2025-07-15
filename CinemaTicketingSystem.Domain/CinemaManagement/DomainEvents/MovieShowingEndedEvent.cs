namespace CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents;

public record MovieShowingEndedEvent(Guid MovieId, string MovieTitle, DateTime EndDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}