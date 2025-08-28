#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing;

internal class TicketIssuanceRepository(AppDbContext context)
    : GenericRepository<TicketIssuance>(context), ITicketIssuanceRepository
{
    public async Task<List<TicketIssuance>> GetConfirmedTicketsIssuanceByScheduleIdAndScreeningDate(
        Guid scheduledMovieShowId, DateOnly screeningDate)
    {
        return await _context.TicketIssuance
            .Where(x => x.ScreeningDate == screeningDate && x.ScheduledMovieShowId == scheduledMovieShowId &&
                        x.Status == TicketIssuanceStatus.Confirmed)
            .ToListAsync();
    }

    public async Task<TicketIssuance?> Get(Guid CustomerId, DateOnly ScreeningDate, Guid ScheduledMovieShowId)
    {
        var localTicketIssuance = _context.TicketIssuance.Local.FirstOrDefault(x =>
            x.CustomerId == CustomerId && x.ScreeningDate == ScreeningDate &&
            x.ScheduledMovieShowId == ScheduledMovieShowId);

        if (localTicketIssuance is not null) return localTicketIssuance;

        return await _context.TicketIssuance.FirstOrDefaultAsync(x =>
            x.CustomerId == CustomerId && x.ScreeningDate == ScreeningDate &&
            x.ScheduledMovieShowId == ScheduledMovieShowId);
    }


    public Task<TicketIssuance> Get(Guid customerId, Guid TicketIssuanceId)
    {
        return _context.TicketIssuance.SingleAsync(x => x.CustomerId == customerId && x.Id == TicketIssuanceId);
    }

    public List<TicketIssuance> GetTicketsPurchaseByScheduleIdAndScreeningDate(Guid scheduleId, DateOnly ScreeningDate)
    {
        return _context.TicketIssuance
            .Where(x => x.ScheduledMovieShowId == scheduleId && x.ScreeningDate == ScreeningDate)
            .ToList();
    }

    public Task Confirm(Guid customerId, Guid TicketIssuanceId)
    {
        throw new NotImplementedException();
    }
}