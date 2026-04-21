namespace StickedWords.API.Models.Exercises;

public record GuessDto
{
    public long FlashCardId { get; init; }

    public required string Answer { get; init; }
}
