#region

using CinemaTicketingSystem.SharedKernel;
using MassTransit;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Messaging;

public class IntegrationEventBus(IPublishEndpoint publishEndpoint) : IIntegrationEventBus
{
    public Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        return publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}