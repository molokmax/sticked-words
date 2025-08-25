namespace StickedWords.Domain.Exceptions;

public sealed class ActiveStageNotFoundException : Exception
{
    private static readonly string _message = "Not found active stage for learning session";

    public ActiveStageNotFoundException() : base(_message) { }
}
