#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.Specifications;

/// <summary>
/// Specification for seat holds filtered by customer, scheduled movie show, and screening date.
/// </summary>
public sealed class UserSeatHoldsByScheduleAndDateSpec : Specification<SeatHold>
{
    public UserSeatHoldsByScheduleAndDateSpec(Guid customerId, Guid scheduledMovieShowId, DateOnly screeningDate)
    {
        AddCriteria(x =>
            x.CustomerId == customerId &&
            x.ScheduledMovieShowId == scheduledMovieShowId &&
            x.ScreeningDate == screeningDate);
    }
}
