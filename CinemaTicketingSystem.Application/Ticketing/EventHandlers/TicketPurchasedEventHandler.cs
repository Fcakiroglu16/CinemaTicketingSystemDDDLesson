using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using CinemaTicketingSystem.Domain.Ticketing.DomainEvents;

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class TicketPurchasedEventHandler(ISeatHoldRepository seatHoldRepository) : IEventHandler<TicketPurchasedEvent>
{
    public async Task HandleAsync(TicketPurchasedEvent message, CancellationToken cancellationToken = default)
    {
        // Logic to handle the ticket purchased event
        // For example, you might want to remove the seat hold for the purchased tickets


        var seatHoldToDelete = await seatHoldRepository.GetAsync(
            x => x.ScheduledMovieShowId == message.ScheduledMovieShowId && x.CustomerId == message.CustomerId &&
                 x.SeatPosition == message.SeatPosition, cancellationToken);

        if (seatHoldToDelete is null)
            throw new BusinessException(ErrorCodes.SeatHoldNotFound).AddData(message.SeatPosition.Row)
                .AddData(message.SeatPosition.Number);

        if (!seatHoldToDelete.CanBeConvertedToReservationOrPurchase())
            throw new BusinessException(ErrorCodes.SeatHoldExpired)
                .AddData(message.SeatPosition.Row)
                .AddData(message.SeatPosition.Number);

        await seatHoldRepository.DeleteAsync(seatHoldToDelete, cancellationToken);
    }
}