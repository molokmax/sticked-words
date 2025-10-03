using System.Linq.Expressions;

namespace StickedWords.Domain.Specifications;

public abstract class Specification<TEntity> : ISpecification<TEntity>
{
    private readonly List<Ordering<TEntity>> _orders = [];

    protected Specification() { }

    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void SetPaging(int skip, int? take)
    {
        Skip = skip;
        Take = take;
    }

    protected void AddOrder(
        Expression<Func<TEntity, object>> fieldSelector,
        OrderDirection direction = OrderDirection.Ascending)
    {
        _orders.Add(new(fieldSelector, direction));
    }

    public Expression<Func<TEntity, bool>>? Criteria { get; protected set; }

    public IEnumerable<Ordering<TEntity>> Orders => _orders;

    public int? Take { get; private set; }

    public int Skip { get; private set; }
}
