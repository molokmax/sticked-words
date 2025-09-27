namespace StickedWords.Domain.Models;

public record LearningSessionResults
{
    public LearningSessionState State { get; init; }

    public int FlashCardCount { get; init; }
}
