using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.DomainEvents;

public record TicketPurchasedEvent(
    Guid TicketId,
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    SeatPosition SeatPosition,
    Price Price) : IDomainEvent;