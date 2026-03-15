#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Specifications;

/// <summary>
/// Specification that checks whether a movie with the given title exists.
/// </summary>
public sealed class MovieByTitleSpec : Specification<Movie>
{
    public MovieByTitleSpec(string title)
    {
        AddCriteria(m => m.Title.Equals(title));
    }
}
