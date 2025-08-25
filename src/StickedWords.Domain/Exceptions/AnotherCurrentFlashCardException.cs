namespace StickedWords.Domain.Exceptions;

public sealed class AnotherCurrentFlashCardException : Exception
{
    private static readonly string _message = "Specified flash card is not current flash card in learning session";

    public AnotherCurrentFlashCardException() : base(_message) { }
}
