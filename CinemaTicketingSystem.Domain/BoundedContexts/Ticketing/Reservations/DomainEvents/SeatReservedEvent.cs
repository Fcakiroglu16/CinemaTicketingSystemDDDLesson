using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record SeatReservedEvent(
    Guid ReservationId,
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    SeatPosition SeatPosition) : IDomainEvent;