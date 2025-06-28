namespace StickedWords.Domain.Models;

public record SessionFlashCard
{
    public long Id { get; init; }

    public long LearningSessionId { get; init; }

    // TODO: везде проверь навигационные поля и проверь что они действительно нужны
    public required virtual LearningSession LearningSession { get; init; } // TODO: это здесь нужно?

    public long FlashCardId { get; init; }

    public required virtual FlashCard FlashCard { get; init; } // TODO: это здесь нужно?
}
