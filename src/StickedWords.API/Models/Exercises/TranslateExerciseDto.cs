namespace StickedWords.API.Models.Exercises;

public record TranslateExerciseDto
{
    public required string Word { get; init; }

    public required long? ImageId { get; init; }
}
