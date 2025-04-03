namespace StickedWords.API.Models;

public record FlashCardShortDto
{
    public long Id { get; init; }

    public required string Word { get; init; }

    public required string Translation { get; init; }
}
