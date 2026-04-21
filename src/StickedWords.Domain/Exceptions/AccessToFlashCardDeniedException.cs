namespace StickedWords.Domain.Exceptions;

public sealed class AccessToFlashCardDeniedException : Exception
{
    private static readonly string _message = "Access denied to flash card [{0}] for user [{1}]";

    public AccessToFlashCardDeniedException(long flashCardId, long userId)
        : base(string.Format(_message, flashCardId, userId))
    {
        FlashCardId = flashCardId;
        UserId = userId;
    }

    public long FlashCardId { get; }

    public long UserId { get; }
}
