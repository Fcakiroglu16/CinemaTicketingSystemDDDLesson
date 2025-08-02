using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.DomainEvents;

public record SeatReservationReleasedEvent(Guid ReservationId, SeatPosition SeatPosition) : IDomainEvent;