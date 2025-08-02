using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.Catalog.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketingSystem.Persistence.Catalog;

public class CinemaRepository(AppDbContext context) : GenericRepository<Guid, Cinema>(context), ICinemaRepository

{
    public Task<Cinema?> GetByHallId(Guid hallId)
    {
        return _context.Cinemas
            .Include(c => c.Halls)
            .FirstOrDefaultAsync(c => c.Halls.Any(h => h.Id == hallId));
    }
}