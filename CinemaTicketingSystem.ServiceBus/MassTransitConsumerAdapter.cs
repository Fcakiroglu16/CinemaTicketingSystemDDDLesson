using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.SharedKernel;
using MassTransit;

namespace CinemaTicketingSystem.ServiceBus;

public class MassTransitConsumerAdapter<TMessage>(IIntegrationEventHandler<TMessage> handler) : IConsumer<TMessage>
    where TMessage : class, IIntegrationEvent
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await handler.HandleAsync(context.Message, context.CancellationToken);
    }
}
