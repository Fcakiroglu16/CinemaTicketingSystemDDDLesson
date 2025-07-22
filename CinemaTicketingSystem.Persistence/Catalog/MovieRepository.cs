using CinemaTicketingSystem.Domain.Catalog;
using CinemaTicketingSystem.Domain.Catalog.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketingSystem.Persistence.Catalog;

internal class MovieRepository(AppDbContext context) : GenericRepository<Guid, Movie>(context), IMovieRepository
{
    public Task<bool> CheckIfMovieExists(string title)
    {
        return _context.Movies.AnyAsync(m =>
            m.Title.Equals(title));
    }
}