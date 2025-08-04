using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases.DomainEvents;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.Exceptions;
using MediatR;

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class TicketPurchasedEventHandler(ISeatHoldRepository seatHoldRepository) : INotificationHandler<TicketPurchasedEvent>
{


    public async Task Handle(TicketPurchasedEvent notification, CancellationToken cancellationToken)
    {

        var x = await seatHoldRepository.GetAllAsync();


        var seatHoldToDelete = await seatHoldRepository.GetAsync(
            x => x.ScheduledMovieShowId == notification.ScheduledMovieShowId && x.CustomerId == notification.CustomerId && x.SeatPosition.Row == notification.SeatPosition.Row && x.SeatPosition.Number == notification.SeatPosition.Number, cancellationToken);

        var seatHoldToDelete2 = await seatHoldRepository.GetAsync(
            x => x.ScheduledMovieShowId == notification.ScheduledMovieShowId && x.CustomerId == notification.CustomerId && x.SeatPosition.Equals(notification.SeatPosition), cancellationToken);


        if (seatHoldToDelete is null)
            throw new BusinessException(ErrorCodes.SeatHoldNotFound).AddData(notification.SeatPosition.Row)
                .AddData(notification.SeatPosition.Number);

        if (!seatHoldToDelete.CanBeConvertedToReservationOrPurchase())
            throw new BusinessException(ErrorCodes.SeatHoldExpired)
                .AddData(notification.SeatPosition.Row)
                .AddData(notification.SeatPosition.Number);

        await seatHoldRepository.DeleteAsync(seatHoldToDelete, cancellationToken);
    }
}