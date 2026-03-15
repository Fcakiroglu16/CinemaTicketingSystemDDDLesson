namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.Specifications;

/// <summary>
/// Specification for confirmed ticket issuances filtered by scheduled movie show and screening date.
/// </summary>
public sealed class ConfirmedTicketIssuanceByScheduleAndDateSpec : Repositories.Specification<TicketIssuance>
{
    public ConfirmedTicketIssuanceByScheduleAndDateSpec(Guid scheduledMovieShowId, DateOnly screeningDate)
    {
        AddCriteria(x =>
            x.ScreeningDate == screeningDate &&
            x.ScheduledMovieShowId == scheduledMovieShowId &&
            x.Status == TicketIssuanceStatus.Confirmed);
    }
}
