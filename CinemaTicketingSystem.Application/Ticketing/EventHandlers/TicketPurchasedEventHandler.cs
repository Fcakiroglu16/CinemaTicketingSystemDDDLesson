#region

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

//public class TicketPurchasedEventHandler(ISeatHoldRepository seatHoldRepository)
//    : INotificationHandler<TicketIssuanceCreatedEvent>
//{
//    public async Task Handle(TicketIssuanceCreatedEvent notification, CancellationToken cancellationToken)
//    {
//        var seatHoldToDelete = await seatHoldRepository.GetAsync(
//            x => x.ScheduledMovieShowId == notification.ScheduledMovieShowId &&
//                 x.CustomerId == notification.CustomerId && x.SeatPosition.Row == notification.SeatPosition.Row &&
//                 x.SeatPosition.Number == notification.SeatPosition.Number, cancellationToken);

//        //var seatHoldToDelete2 = await seatHoldRepository.GetAsync(
//        //    x => x.ScheduledMovieShowId == notification.ScheduledMovieShowId && x.CustomerId == notification.CustomerId && x.SeatPosition.Equals(notification.SeatPosition), cancellationToken);


//        await seatHoldRepository.DeleteAsync(seatHoldToDelete!, cancellationToken);
//    }
//}