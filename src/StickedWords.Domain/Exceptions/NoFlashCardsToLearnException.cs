namespace StickedWords.Domain.Exceptions;

public sealed class NoFlashCardsToLearnException : Exception
{
    private static readonly string _message = "There is no flash cards to learn";

    public NoFlashCardsToLearnException() : base(_message)
    {
    }
}
