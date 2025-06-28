namespace StickedWords.Domain;

public record LearningSessionOptions
{
    public const string SectionName = "LearningSession";

    public TimeSpan ExpireAfter { get; init; } = TimeSpan.FromHours(3);

    public int FlashCardCount { get; init; } = 10;
}
