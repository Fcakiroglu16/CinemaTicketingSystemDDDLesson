#region

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

//public class SeatReservedEventHandler(ISeatHoldRepository seatHoldRepository) : INotificationHandler<SeatReservedEvent>
//{
//    public async Task Handle(SeatReservedEvent notification, CancellationToken cancellationToken)
//    {
//        var seatHoldToDelete = await seatHoldRepository.GetAsync(
//            x => x.ScheduledMovieShowId == notification.ScheduledMovieShowId &&
//                 x.CustomerId == notification.CustomerId &&
//                 x.SeatPosition.Number == notification.SeatPosition.Number &&
//                 x.SeatPosition.Row == notification.SeatPosition.Row, cancellationToken);
//        ;

//        await seatHoldRepository.DeleteAsync(seatHoldToDelete!, cancellationToken);
//    }
//}