namespace CinemaTicketingSystem.Domain.Catalog.DomainEvents;

public record MovieAssignedToHallEvent(Guid MovieId, Guid HallId, string HallName, DateTime AssignedAt)
    : IDomainEvent
{
}