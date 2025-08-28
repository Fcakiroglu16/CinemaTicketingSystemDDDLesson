#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;

public interface ITicketIssuanceRepository : IGenericRepository<TicketIssuance>
{
    Task<List<TicketIssuance>> GetConfirmedTicketsIssuanceByScheduleIdAndScreeningDate(Guid scheduleId,
        DateOnly ScreeningDate);


    Task<TicketIssuance?> Get(Guid CustomerId, DateOnly ScreeningDate, Guid ScheduledMovieShowId);

    Task<TicketIssuance> Get(Guid customerId, Guid TicketIssuanceId);
}