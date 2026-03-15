namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.Specifications;

/// <summary>
/// Specification for a ticket issuance filtered by customer, screening date, and scheduled movie show.
/// </summary>
public sealed class TicketIssuanceByCustomerAndScheduleSpec : Repositories.Specification<TicketIssuance>
{
    public TicketIssuanceByCustomerAndScheduleSpec(Guid customerId, DateOnly screeningDate,
        Guid scheduledMovieShowId)
    {
        AddCriteria(x =>
            x.CustomerId == customerId &&
            x.ScreeningDate == screeningDate &&
            x.ScheduledMovieShowId == scheduledMovieShowId);
    }
}
