namespace StickedWords.Domain.Exceptions;

public sealed class SessionStageNotInSessionException : Exception
{
    private static readonly string _message = "Stage is not in session";

    public SessionStageNotInSessionException() : base(_message) { }
}
