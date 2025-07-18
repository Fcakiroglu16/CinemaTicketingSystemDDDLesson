namespace CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents
{
    public record MovieAssignedToHallEvent(Guid MovieId, Guid HallId, string HallName, DateTime AssignedAt)
        : IDomainEvent;


