namespace StickedWords.Domain.Models;

public record Guess
{
    public long Id { get; init; }

    public long SessionStageId { get; init; }

    public required virtual SessionStage SessionStage { get; init; }

    public long FlashCardId { get; init; }

    public required virtual FlashCard FlashCard { get; init; }

    public GuessResult Result { get; init; }
}
