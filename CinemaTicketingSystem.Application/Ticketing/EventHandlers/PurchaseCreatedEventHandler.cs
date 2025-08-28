#region

using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class TicketPurchasedEventHandler(
    IUnitOfWork unitOfWork,
    ITicketIssuanceRepository ticketIssuanceRepository,
    ISeatHoldRepository seatHoldRepository)
    : IIntegrationEventHandler<PurchaseCreatedEvent>
{
    public async Task HandleAsync(PurchaseCreatedEvent message, CancellationToken cancellationToken = default)
    {
        var ticketIssuance = await ticketIssuanceRepository.Get(message.userId, message.TicketIssuanceId);

        ticketIssuance.Confirm();

        var customerSeatHoldList = await seatHoldRepository.WhereAsync(x =>
            x.CustomerId == message.userId && x.ScheduledMovieShowId == ticketIssuance.ScheduledMovieShowId &&
            x.ScreeningDate == ticketIssuance.ScreeningDate, cancellationToken);

        foreach (var customerSeatHold in customerSeatHoldList)
        {
            await seatHoldRepository.DeleteAsync(customerSeatHold, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}