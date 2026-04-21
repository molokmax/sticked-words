namespace StickedWords.API.Models;

public record ChooseExerciseDto
{
    public required string Word { get; init; }

    public required long? ImageId { get; init; }

    public required string[] Options { get; init; }
}
