namespace StickedWords.Domain.Exceptions;

public sealed class CurrentFlashCardNotDefinedException : Exception
{
    private static readonly string _message = "Current flash card is not defined";

    public CurrentFlashCardNotDefinedException(long sessionStageId) : base(_message)
    {
        SessionStageId = sessionStageId;
    }

    public long SessionStageId { get; }
}
