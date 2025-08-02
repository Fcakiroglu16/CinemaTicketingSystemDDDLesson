using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations.DomainEvents;

public record ReservationCreatedEvent(
    Guid ReservationId,
    Guid CustomerId,
    Guid ScheduledMovieShowId,
    DateTime ReservationTime)
    : IDomainEvent;