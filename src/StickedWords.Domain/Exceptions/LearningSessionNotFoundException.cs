namespace StickedWords.Domain.Exceptions;

public sealed class LearningSessionNotFoundException : Exception
{
    private static readonly string _message = "Learning session is not found";

    public LearningSessionNotFoundException(long sessionId) : base(_message)
    {
        LearningSessionId = sessionId;
    }

    public long LearningSessionId { get; }
}
