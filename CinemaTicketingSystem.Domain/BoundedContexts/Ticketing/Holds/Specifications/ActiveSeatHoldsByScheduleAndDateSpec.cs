#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.Specifications;

/// <summary>
/// Specification for active (held and not expired) seat holds filtered by scheduled movie show and screening date.
/// </summary>
public sealed class ActiveSeatHoldsByScheduleAndDateSpec : Specification<SeatHold>
{
    public ActiveSeatHoldsByScheduleAndDateSpec(Guid scheduledMovieShowId, DateOnly screeningDate)
    {
        AddCriteria(x =>
            x.ScheduledMovieShowId == scheduledMovieShowId &&
            x.ScreeningDate == screeningDate &&
            x.Status == HoldStatus.Hold &&
            x.ExpiresAt > DateTime.UtcNow);
    }
}
