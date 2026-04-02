namespace StickedWords.API.Models;

public record CreateFlashCardRequestDto
{
    public required string Word { get; init; }

    public required string Translation { get; init; }

    public required long? ImageId { get; init; }
}
