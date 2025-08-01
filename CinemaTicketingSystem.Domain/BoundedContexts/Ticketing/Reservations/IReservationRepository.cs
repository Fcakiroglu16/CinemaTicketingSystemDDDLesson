using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;

public interface IReservationRepository : IGenericRepository<Guid, Reservation>
{
}