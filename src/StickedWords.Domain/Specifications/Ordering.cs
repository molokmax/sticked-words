using System.Linq.Expressions;

namespace StickedWords.Domain.Specifications;

public record Ordering<TEntity>(Expression<Func<TEntity, object>> FieldSelector, OrderDirection Direction);
