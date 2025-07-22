namespace CinemaTicketingSystem.Application.Contracts;

public interface IEventHandler<in TEvent>
    where TEvent : class
{
    Task HandleAsync(TEvent message, CancellationToken cancellationToken = default);
}