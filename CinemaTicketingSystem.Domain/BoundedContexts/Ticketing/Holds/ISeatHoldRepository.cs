#region

using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

public interface ISeatHoldRepository : IGenericRepository<SeatHold>
{
    Task DeleteByCustomerAndSeatsAsync(Guid customerId, List<SeatPosition> seatPositions, DateOnly screeningDate,
        CancellationToken cancellationToken = default);
}