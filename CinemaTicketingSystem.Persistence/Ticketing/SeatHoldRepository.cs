#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.ValueObjects;
using CinemaTicketingSystem.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing;

public class SeatHoldRepository(AppDbContext context) : GenericRepository<SeatHold>(context), ISeatHoldRepository
{
    public Task<List<SeatHold>> GetConfirmedListByScheduleIdAndScreeningDate(Guid scheduledMovieShowId,
        DateOnly screeningDate)
    {
        return _context.SeatHolds
            .Where(x => x.ScheduledMovieShowId == scheduledMovieShowId && x.ScreeningDate == screeningDate &&
                        x.Status == HoldStatus.Hold && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task DeleteByCustomerAndSeatsAsync(Guid customerId, List<SeatPosition> seatPositions,
        DateOnly screeningDate,
        CancellationToken cancellationToken = default)
    {
        var customerIdValue = new CustomerId(customerId);

        var seatHoldsToDelete = await _context.SeatHolds
            .Where(x => x.CustomerId == customerIdValue && x.ScreeningDate == screeningDate)
            .ToListAsync(cancellationToken);

        var matchedSeatHolds = seatHoldsToDelete
            .Where(x => seatPositions.Any(sp => sp.Row == x.SeatPosition.Row && sp.Number == x.SeatPosition.Number))
            .ToList();

        context.SeatHolds.RemoveRange(matchedSeatHolds);
    }
}