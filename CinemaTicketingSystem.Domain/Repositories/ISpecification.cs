#region

using CinemaTicketingSystem.SharedKernel.Entities;
using System.Linq.Expressions;

#endregion

namespace CinemaTicketingSystem.Domain.Repositories;

/// <summary>
/// Defines a query specification that encapsulates filtering, ordering, and include logic for a given entity.
/// </summary>
public interface ISpecification<TEntity> where TEntity : EntityBase
{
    /// <summary>
    /// Gets the filter criteria expression.
    /// </summary>
    Expression<Func<TEntity, bool>>? Criteria { get; }

    /// <summary>
    /// Gets the list of include expressions for eager loading related entities.
    /// </summary>
    IReadOnlyList<Expression<Func<TEntity, object>>> Includes { get; }

    /// <summary>
    /// Gets the ordering expression for ascending sort.
    /// </summary>
    Expression<Func<TEntity, object>>? OrderBy { get; }

    /// <summary>
    /// Gets the ordering expression for descending sort.
    /// </summary>
    Expression<Func<TEntity, object>>? OrderByDescending { get; }
}
