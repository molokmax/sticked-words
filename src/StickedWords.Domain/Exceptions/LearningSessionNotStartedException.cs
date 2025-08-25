namespace StickedWords.Domain.Exceptions;

public sealed class LearningSessionNotStartedException : Exception
{
    private static readonly string _message = "Learning session is not started";

    public LearningSessionNotStartedException() : base(_message) { }
}
