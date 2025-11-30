#region

using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Contracts;

public interface IIntegrationEventHandler<in TEvent>
    where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent message, CancellationToken cancellationToken = default);
}