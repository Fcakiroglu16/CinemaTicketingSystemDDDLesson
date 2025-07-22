using CinemaTicketingSystem.Domain.Catalog;

namespace CinemaTicketingSystem.Domain.Repositories;

public interface ICinemaRepository : IGenericRepository<Guid, Cinema>
{
}