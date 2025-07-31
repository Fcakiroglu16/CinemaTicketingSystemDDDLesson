using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.Catalog.Repositories;

public interface ICinemaRepository : IGenericRepository<Guid, Cinema>
{
    Task<Cinema?> GetByHallId(Guid hallId);
}