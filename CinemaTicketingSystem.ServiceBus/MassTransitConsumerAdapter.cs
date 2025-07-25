using CinemaTicketingSystem.Application.Abstraction.Contracts;
using MassTransit;

namespace CinemaTicketingSystem.ServiceBus;

public class MassTransitConsumerAdapter<TMessage>(IEventHandler<TMessage> handler) : IConsumer<TMessage>
    where TMessage : class
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await handler.HandleAsync(context.Message, context.CancellationToken);
    }
}