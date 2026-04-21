using StickedWords.Domain.Models;

namespace StickedWords.API.Models.Exercises;

public record GuessResultDto
{
    public Verdict Result { get; init; }

    public string? CorrectTranslation { get; init; }

    public bool IsExpired { get; init; }
}
