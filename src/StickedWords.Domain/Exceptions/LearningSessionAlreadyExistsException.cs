namespace StickedWords.Domain.Exceptions;

public sealed class LearningSessionAlreadyExistsException : Exception
{
    private static readonly string _message = "Learning session already started";

    public LearningSessionAlreadyExistsException() : base(_message) { }
}
