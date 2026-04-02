namespace StickedWords.Domain.Models.Exercises;

public record TranslateExercise
{
    public required string Word { get; init; }

    public required long? ImageId { get; init; }
}
