using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.Catalog.Repositories;

public interface IMovieRepository : IGenericRepository<Guid, Movie>
{
    Task<bool> CheckIfMovieExists(string title);
}