namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.Specifications;

/// <summary>
/// Specification for a ticket issuance filtered by customer ID and ticket issuance ID.
/// </summary>
public sealed class TicketIssuanceByCustomerAndIdSpec : Repositories.Specification<TicketIssuance>
{
    public TicketIssuanceByCustomerAndIdSpec(Guid customerId, Guid ticketIssuanceId)
    {
        AddCriteria(x => x.CustomerId == customerId && x.Id == ticketIssuanceId);
    }
}
