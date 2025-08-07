#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases.DomainEvents;
using MediatR;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class TicketPurchasedEventHandler(ISeatHoldRepository seatHoldRepository)
    : INotificationHandler<TicketPurchasedEvent>
{
    public async Task Handle(TicketPurchasedEvent notification, CancellationToken cancellationToken)
    {
        var seatHoldToDelete = await seatHoldRepository.GetAsync(
            x => x.ScheduledMovieShowId == notification.ScheduledMovieShowId &&
                 x.CustomerId == notification.CustomerId && x.SeatPosition.Row == notification.SeatPosition.Row &&
                 x.SeatPosition.Number == notification.SeatPosition.Number, cancellationToken);

        //var seatHoldToDelete2 = await seatHoldRepository.GetAsync(
        //    x => x.ScheduledMovieShowId == notification.ScheduledMovieShowId && x.CustomerId == notification.CustomerId && x.SeatPosition.Equals(notification.SeatPosition), cancellationToken);


        await seatHoldRepository.DeleteAsync(seatHoldToDelete!, cancellationToken);
    }
}