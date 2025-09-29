using StickedWords.Domain.Models;

namespace StickedWords.API.Models;

public record LearningSessionDto
{
    public long Id { get; init; }

    public LearningSessionState State { get; init; }

    public ExerciseType ExerciseType { get; init; }

    public long? FlashCardId { get; init; }

    public int FlashCardCount { get; init; }

    public IReadOnlyCollection<LearningSessionStageDto> Stages { get; init; } = [];
}
