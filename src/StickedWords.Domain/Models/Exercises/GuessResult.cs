namespace StickedWords.Domain.Models.Exercises;

public record GuessResult : IExerciseResult
{
    private GuessResult() { }

    public Verdict Result { get; init; }

    public string? CorrectTranslation { get; init; }

    public required bool IsExpired { get; init; }

    public static GuessResult Correct(LearningSessionState sessionState)
    {
        return new()
        {
            Result = Verdict.Correct,
            IsExpired = sessionState is LearningSessionState.Expired
        };
    }

    public static GuessResult Wrong(FlashCardWord translation, LearningSessionState sessionState)
    {
        return new()
        {
            Result = Verdict.Wrong,
            CorrectTranslation = translation.Word,
            IsExpired = sessionState is LearningSessionState.Expired
        };
    }
}
