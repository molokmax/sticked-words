namespace StickedWords.Domain.Exceptions;

public sealed class UserNotFoundException : Exception
{
    private static readonly string _message = "User not found";

    public UserNotFoundException(long userId) : base(_message)
    {
        UserId = userId;
    }

    public long UserId { get; }
}
