using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases.DomainEvents;

public record TicketPurchasedEvent(
    Guid TicketId,
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    SeatPosition SeatPosition,
    Price Price) : IDomainEvent;