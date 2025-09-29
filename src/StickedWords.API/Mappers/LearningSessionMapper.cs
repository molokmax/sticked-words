using StickedWords.API.Models;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.API.Mappers;

internal static class LearningSessionMapper
{
    public static LearningSessionDto ToDto(this LearningSession source)
    {
        var activeStage = source.Stages.FirstOrDefault(x => x.IsActive);

        return new()
        {
            Id = source.Id,
            State = source.State,
            ExerciseType = activeStage?.ExerciseType ?? ExerciseType.None,
            FlashCardId = activeStage?.CurrentFlashCard?.FlashCardId,
            FlashCardCount = source.FlashCards.Count,
            Stages = source.Stages
                .OrderBy(x => x.OrdNumber)
                .Select(x => new LearningSessionStageDto
                {
                    Id = x.Id,
                    OrdNumber = x.OrdNumber,
                    CompletedFlashCardCount = x.Guesses.Count
                })
                .ToArray()
        };
    }

    public static TranslateExerciseDto ToDto(this TranslateExercise source) =>
        new()
        {
            Word = source.Word
        };

    public static LearningSessionResultsDto ToDto(this LearningSessionResults source)
    {
        return new()
        {
            State = source.State,
            FlashCardCount = source.FlashCardCount
        };
    }
}
