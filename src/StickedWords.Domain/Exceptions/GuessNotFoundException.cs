namespace StickedWords.Domain.Exceptions;

public sealed class GuessNotFoundException : Exception
{
    private static readonly string _message = "Guess not found for flash card";

    public GuessNotFoundException(long flashCardId) : base(_message)
    {
        FlashCardId = flashCardId;
    }


    public long FlashCardId { get; }
}
