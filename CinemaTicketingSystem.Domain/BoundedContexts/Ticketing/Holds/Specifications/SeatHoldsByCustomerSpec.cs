#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.Specifications;

/// <summary>
/// Specification for seat holds filtered by customer ID.
/// </summary>
public sealed class SeatHoldsByCustomerSpec : Specification<SeatHold>
{
    public SeatHoldsByCustomerSpec(Guid customerId)
    {
        AddCriteria(x => x.CustomerId == customerId);
    }
}
