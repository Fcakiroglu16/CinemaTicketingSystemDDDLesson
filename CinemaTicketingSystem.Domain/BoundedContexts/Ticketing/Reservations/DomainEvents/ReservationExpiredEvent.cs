using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations.DomainEvents;

public record ReservationExpiredEvent(Guid ReservationId, Guid CustomerId, Guid ScheduledMovieShowId) : IDomainEvent;