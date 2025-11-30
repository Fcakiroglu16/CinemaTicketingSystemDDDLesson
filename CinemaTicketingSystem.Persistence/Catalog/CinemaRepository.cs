#region

using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Catalog;

public class CinemaRepository(AppDbContext context) : GenericRepository<Cinema>(context), ICinemaRepository

{
    public Task<Cinema?> GetByHallId(Guid hallId)
    {
        return _context.Cinemas
            .Include(c => c.Halls)
            .FirstOrDefaultAsync(c => c.Halls.Any(h => h.Id == hallId));
    }
}