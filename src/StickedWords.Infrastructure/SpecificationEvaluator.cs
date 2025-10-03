using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Specifications;

namespace StickedWords.Infrastructure;

internal static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetFilteredQuery<TEntity>(
        this IQueryable<TEntity> source, ISpecification<TEntity> specification)
    {
        var query = source;
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        return query;
    }

    public static IQueryable<TEntity> GetQuery<TEntity>(
        this IQueryable<TEntity> source, ISpecification<TEntity> specification)
    {
        var query = source.GetFilteredQuery(specification);

        foreach (var ordering in specification.Orders)
        {
            query = ordering.Direction switch
            {
                OrderDirection.Ascending => query.OrderBy(ordering.FieldSelector),
                OrderDirection.Descending => query.OrderByDescending(ordering.FieldSelector),
                _ => throw new OrderDirectionIsNotSupportedException(ordering.Direction)
            };
        }

        if (specification.Skip > 0)
        {
            query = query.Skip(specification.Skip);
        }

        if (specification.Take is not null)
        {
            query = query.Take(specification.Take.Value);
        }

        return query;
    }
}
