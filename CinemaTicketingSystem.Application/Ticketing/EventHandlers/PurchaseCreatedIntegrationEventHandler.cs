#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class PurchaseCreatedIntegrationEventHandler(
    IUnitOfWork unitOfWork,
    ITicketIssuanceRepository ticketIssuanceRepository,
    ISeatHoldRepository seatHoldRepository)
    : IIntegrationEventHandler<PurchaseCreatedIntegrationEvent>
{
    public async Task HandleAsync(PurchaseCreatedIntegrationEvent message,
        CancellationToken cancellationToken = default)
    {
        TicketIssuance ticketIssuance = await ticketIssuanceRepository.Get(message.userId, message.TicketIssuanceId);

        ticketIssuance.Confirm();

        IEnumerable<SeatHold> customerSeatHoldList = await seatHoldRepository.WhereAsync(x =>
            x.CustomerId == message.userId && x.ScheduledMovieShowId == ticketIssuance.ScheduledMovieShowId &&
            x.ScreeningDate == ticketIssuance.ScreeningDate, cancellationToken);

        foreach (SeatHold customerSeatHold in customerSeatHoldList)
            await seatHoldRepository.DeleteAsync(customerSeatHold, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}