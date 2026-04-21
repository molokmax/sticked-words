namespace StickedWords.Domain.Models.Exercises;

public record ChooseExercise
{
    public required string Word { get; init; }

    public required long? ImageId { get; init; }

    public required string[] Options { get; init; }
}
