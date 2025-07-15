using CinemaTicketingSystem.Domain.CinemaManagement;
using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Persistence.CinemaManagement
{
    internal class MovieRepository(AppDbContext context) : GenericRepository<Guid, Movie>(context), IMovieRepository
    {






    }
}
