using CinemaTicketingSystem.Domain.Catalog;
using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Persistence.Catalog;

public class CinemaRepository(AppDbContext context) : GenericRepository<Guid, Cinema>(context), ICinemaRepository

{
}