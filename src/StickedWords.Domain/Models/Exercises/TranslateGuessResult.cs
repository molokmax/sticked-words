namespace StickedWords.Domain.Models.Exercises;

public record TranslateGuessResult : IExerciseResult
{
    private TranslateGuessResult() { }

    public GuessResult Result { get; init; }

    public string? CorrectTranslation { get; init; }

    public required bool IsExpired { get; init; }

    public static TranslateGuessResult Correct(LearningSessionState sessionState)
    {
        return new()
        {
            Result = GuessResult.Correct,
            IsExpired = sessionState is LearningSessionState.Expired
        };
    }

    public static TranslateGuessResult Wrong(FlashCardWord translation, LearningSessionState sessionState)
    {
        return new()
        {
            Result = GuessResult.Wrong,
            CorrectTranslation = translation.Word,
            IsExpired = sessionState is LearningSessionState.Expired
        };
    }
}
