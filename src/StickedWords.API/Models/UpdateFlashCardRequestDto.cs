namespace StickedWords.API.Models;

public record UpdateFlashCardRequestDto
{
    public required long Id { get; init; }

    public required string Word { get; init; }

    public required string Translation { get; init; }

    public required long? ImageId { get; init; }
}
