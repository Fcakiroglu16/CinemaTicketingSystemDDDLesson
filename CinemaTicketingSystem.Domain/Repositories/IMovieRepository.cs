using CinemaTicketingSystem.Domain.CinemaManagement;

namespace CinemaTicketingSystem.Domain.Repositories
{
    public interface IMovieRepository : IGenericRepository<Guid, Movie>
    {
    }
}
