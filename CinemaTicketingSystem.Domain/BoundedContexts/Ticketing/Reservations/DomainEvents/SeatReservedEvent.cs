using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations.DomainEvents;

public record SeatReservedEvent(
    Guid ReservationId,
    Guid ScheduledMovieShowId,
    Guid CustomerId,
    SeatPosition SeatPosition) : IDomainEvent;