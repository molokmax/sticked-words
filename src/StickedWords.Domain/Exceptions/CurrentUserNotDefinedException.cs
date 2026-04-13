namespace StickedWords.Domain.Exceptions;

public sealed class CurrentUserNotDefinedException : Exception
{
    private static readonly string _message = "Current user is not defined";

    public CurrentUserNotDefinedException() : base(_message)
    {
    }
}
