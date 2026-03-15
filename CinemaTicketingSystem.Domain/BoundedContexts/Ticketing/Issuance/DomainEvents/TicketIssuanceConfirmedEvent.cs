#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.DomainEvents;

public record TicketIssuanceConfirmedEvent(Guid CustomerId, List<SeatPosition> SeatPositions, DateOnly ScreeningDate)
    : IDomainEvent;