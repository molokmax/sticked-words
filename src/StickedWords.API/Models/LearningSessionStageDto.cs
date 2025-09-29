namespace StickedWords.API.Models;

public record LearningSessionStageDto
{
    public long Id { get; init; }

    public int OrdNumber { get; init; }

    public int CompletedFlashCardCount { get; init; }
}
