namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers
{
    //internal class SeatHoldConfirmedHandler(
    //    ITicketIssuanceRepository ticketIssuanceRepository,
    //    IScheduleQueryService scheduleQueryService) : INotificationHandler<SeatHoldConfirmed>
    //{
    //    public async Task Handle(SeatHoldConfirmed notification, CancellationToken cancellationToken)
    //    {
    //        var ticketIssuance = await ticketIssuanceRepository.Get(notification.CustomerId, notification.ScreeningDate,
    //            notification.ScheduledMovieShowId);


    //        var schedule = await scheduleQueryService.GetScheduleInfo(notification.ScheduledMovieShowId);
    //        var newTicket = new Ticket(notification.seatPosition, schedule.Data!.TicketPrice);

    //        if (ticketIssuance is null)
    //        {
    //            ticketIssuance = new TicketIssuance(notification.ScheduledMovieShowId, notification.CustomerId,
    //                notification.ScreeningDate);
    //            ticketIssuance.AddTicket(newTicket);
    //            await ticketIssuanceRepository.AddAsync(ticketIssuance, cancellationToken);
    //        }
    //        else
    //        {
    //            ticketIssuance.AddTicket(newTicket);
    //            //await ticketIssuanceRepository.UpdateAsync(ticketIssuance, cancellationToken);
    //        }
    //    }
    //}
}