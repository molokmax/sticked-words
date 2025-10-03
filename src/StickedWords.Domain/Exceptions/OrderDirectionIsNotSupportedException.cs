using StickedWords.Domain.Specifications;

namespace StickedWords.Domain.Exceptions;

public sealed class OrderDirectionIsNotSupportedException : Exception
{
    private static readonly string _message = "Order direction is not supported";

    public OrderDirectionIsNotSupportedException(OrderDirection direction) : base(_message)
    {
        Direction = direction;
    }


    public OrderDirection Direction { get; }
}
