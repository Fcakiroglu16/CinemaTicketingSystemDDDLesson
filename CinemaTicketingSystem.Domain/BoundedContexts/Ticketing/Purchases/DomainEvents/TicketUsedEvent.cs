using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.Ticketing.DomainEvents;

public record TicketUsedEvent(Guid TicketId, Guid CustomerId, DateTime UsedAt) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}