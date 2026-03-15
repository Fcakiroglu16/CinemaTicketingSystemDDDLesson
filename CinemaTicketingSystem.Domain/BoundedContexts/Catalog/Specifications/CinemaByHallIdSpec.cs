#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Specifications;

/// <summary>
/// Specification for a cinema that contains a hall with the given ID, including halls eagerly loaded.
/// </summary>
public sealed class CinemaByHallIdSpec : Specification<Cinema>
{
    public CinemaByHallIdSpec(Guid hallId)
    {
        AddCriteria(c => c.Halls.Any(h => h.Id == hallId));
        AddInclude(c => c.Halls);
    }
}
