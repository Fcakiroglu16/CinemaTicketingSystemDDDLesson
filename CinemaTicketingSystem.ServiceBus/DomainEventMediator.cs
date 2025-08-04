using CinemaTicketingSystem.SharedKernel;
using MediatR;

namespace CinemaTicketingSystem.ServiceBus;

public class DomainEventMediator(IMediator mediator) : IDomainEventMediator
{
    public Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent
    {
        return mediator.Publish(domainEvent, cancellationToken);
    }
}