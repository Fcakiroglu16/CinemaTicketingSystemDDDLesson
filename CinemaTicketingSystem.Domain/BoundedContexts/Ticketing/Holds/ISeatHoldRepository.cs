#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

public interface ISeatHoldRepository : IGenericRepository<SeatHold>
{
    Task<List<SeatHold>> GetConfirmedListByScheduleIdAndScreeningDate(Guid scheduledMovieShowId,
        DateOnly ScreeningDate);
}