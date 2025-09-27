namespace StickedWords.Domain.Exceptions;

public sealed class LearningSessionNotActiveException : Exception
{
    private static readonly string _message = "Learning session is not active";

    public LearningSessionNotActiveException() : base(_message) { }
}
