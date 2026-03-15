using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.DomainEvents;
using CinemaTicketingSystem.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class TicketIssuanceConfirmedEventDomainEventHandler(
    ISeatHoldRepository seatHoldRepository,
    IUnitOfWork unitOfWork,
    ILogger<TicketIssuanceConfirmedEventDomainEventHandler> logger) : INotificationHandler<TicketIssuanceConfirmedEvent>
{
    public async Task Handle(TicketIssuanceConfirmedEvent notification, CancellationToken cancellationToken)
    {
        await seatHoldRepository.DeleteByCustomerAndSeatsAsync(
            notification.CustomerId,
            notification.SeatPositions,
            notification.ScreeningDate,
            cancellationToken);
        

        logger.LogInformation(
            "Ticket issuance confirmed and seat holds deleted for CustomerId: {CustomerId}, ScreeningDate: {ScreeningDate}, Seats: {SeatCount}",
            notification.CustomerId,
            notification.ScreeningDate,
            notification.SeatPositions.Count);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}