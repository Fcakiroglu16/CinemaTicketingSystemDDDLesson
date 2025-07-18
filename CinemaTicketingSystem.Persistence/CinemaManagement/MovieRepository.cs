using CinemaTicketingSystem.Domain.CinemaManagement;
using CinemaTicketingSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketingSystem.Persistence.CinemaManagement;

internal class MovieRepository(AppDbContext context) : GenericRepository<Guid, Movie>(context), IMovieRepository
{
    public Task<bool> CheckIfMovieExists(string title)
    {


        return _context.Movies.AnyAsync(m =>
             m.Title.Equals(title));


    }


}