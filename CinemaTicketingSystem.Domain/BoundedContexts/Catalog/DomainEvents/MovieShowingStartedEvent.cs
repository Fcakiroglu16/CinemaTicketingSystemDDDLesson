#region

using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;

public record MovieShowingStartedEvent(Guid MovieId, string MovieTitle, DateTime StartDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}