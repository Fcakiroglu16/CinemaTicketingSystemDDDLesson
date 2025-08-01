using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class SeatReservedEventHandler(ISeatHoldRepository seatHoldRepository) : IEventHandler<SeatReservedEvent>
{
    public async Task HandleAsync(SeatReservedEvent message, CancellationToken cancellationToken = default)
    {
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