namespace StickedWords.API.Models;

public record CreateImageRequestDto
{
    public required string Base64Data { get; init; }
}
