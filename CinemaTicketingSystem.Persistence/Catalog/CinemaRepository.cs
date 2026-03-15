#region

using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Catalog;

public class CinemaRepository(AppDbContext context) : GenericRepository<Cinema>(context), ICinemaRepository;