#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;

public interface IMovieRepository : IGenericRepository<Movie>;