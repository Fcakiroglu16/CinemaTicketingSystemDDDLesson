using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.Catalog.DomainEvents;

public record MovieShowingStartedEvent(Guid MovieId, string MovieTitle, DateTime StartDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}