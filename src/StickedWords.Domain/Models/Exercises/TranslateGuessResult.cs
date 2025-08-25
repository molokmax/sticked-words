namespace StickedWords.Domain.Models.Exercises;

public record TranslateGuessResult
{
    private TranslateGuessResult() { }

    public GuessResult Result { get; init; }

    public string? CorrectTranslation { get; init; }

    public static TranslateGuessResult Correct()
    {
        return new()
        {
            Result = GuessResult.Correct
        };
    }

    public static TranslateGuessResult Wrong(string translation)
    {
        return new()
        {
            Result = GuessResult.Wrong,
            CorrectTranslation = translation
        };
    }
}
