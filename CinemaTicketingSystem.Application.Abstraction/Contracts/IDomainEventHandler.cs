#region

using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Contracts;

public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent message, CancellationToken cancellationToken = default);
}