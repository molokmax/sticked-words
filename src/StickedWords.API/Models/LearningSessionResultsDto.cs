using StickedWords.Domain.Models;

namespace StickedWords.API.Models;

public record LearningSessionResultsDto
{
    public LearningSessionState State { get; init; }

    public int FlashCardCount { get; init; }
}
