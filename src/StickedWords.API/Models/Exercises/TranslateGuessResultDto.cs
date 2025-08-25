using StickedWords.Domain.Models;

namespace StickedWords.API.Models.Exercises;

public record TranslateGuessResultDto
{
    public GuessResult Result { get; init; }

    public string? CorrectTranslation { get; init; }
}
