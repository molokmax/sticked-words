namespace StickedWords.Domain.Exceptions;

public sealed class FlashCardNotFoundException : Exception
{
    private static readonly string _message = "Flash card not found by id";

    public FlashCardNotFoundException(long flashCardId) : base(_message)
    {
        FlashCardId = flashCardId;
    }

    public long FlashCardId { get; }
}
