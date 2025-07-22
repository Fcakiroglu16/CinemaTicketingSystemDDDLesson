using CinemaTicketingSystem.Application.Contracts;
using MassTransit;

namespace CinemaTicketingSystem.ServiceBus;

public class IntegrationEventBus(IPublishEndpoint publishEndpoint) : IIntegrationEventBus
{
    public Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : class
    {
        return publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}