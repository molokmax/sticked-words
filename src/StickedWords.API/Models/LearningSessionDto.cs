using StickedWords.Domain.Models;

namespace StickedWords.API.Models;

public record LearningSessionDto
{
    public ExerciseType ExerciseType { get; init; }

    public long? FlashCardId { get; init; }

    public int FlashCardCount { get; init; }
}
