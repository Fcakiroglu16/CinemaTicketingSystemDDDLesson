using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations.DomainEvents;

public record ReservationCanceledEvent(Guid ReservationId, Guid CustomerId, Guid ScheduledMovieShowId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}