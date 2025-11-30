#region

using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;

public record MovieAssignedToHallEvent(Guid MovieId, Guid HallId, string HallName, DateTime AssignedAt)
    : IDomainEvent
{
}