namespace CinemaTicketingSystem.Domain;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}