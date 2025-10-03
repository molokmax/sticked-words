using System.Linq.Expressions;

namespace StickedWords.Domain.Specifications;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }

    IEnumerable<Ordering<TEntity>> Orders { get; }

    int? Take { get; }

    int Skip { get; }
}
