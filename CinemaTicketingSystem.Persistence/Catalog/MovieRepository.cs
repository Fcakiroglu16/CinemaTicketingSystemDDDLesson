#region

using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Catalog;

internal class MovieRepository(AppDbContext context) : GenericRepository<Movie>(context), IMovieRepository;