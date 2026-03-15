#region

using CinemaTicketingSystem.SharedKernel.Entities;
using System.Linq.Expressions;

#endregion

namespace CinemaTicketingSystem.Domain.Repositories;

/// <summary>
/// Base class for building query specifications with fluent configuration of criteria, includes, and ordering.
/// </summary>
public abstract class Specification<TEntity> : ISpecification<TEntity> where TEntity : EntityBase
{
    private readonly List<Expression<Func<TEntity, object>>> _includes = [];

    public Expression<Func<TEntity, bool>>? Criteria { get; private set; }
    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes => _includes.AsReadOnly();
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

    protected void AddCriteria(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        _includes.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }
}
