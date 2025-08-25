namespace StickedWords.API.Models.Exercises;

public record TranslateGuessDto
{
    public long FlashCardId { get; init; }

    public required string Answer { get; init; }
}
