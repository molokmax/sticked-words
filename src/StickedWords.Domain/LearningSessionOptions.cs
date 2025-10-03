using StickedWords.Shared;

namespace StickedWords.Domain;

public record LearningSessionOptions : IConfigurationOptions
{
    public static string SectionName => "LearningSession";

    public TimeSpan ExpireAfter { get; init; } = TimeSpan.FromHours(3);

    public int FlashCardCount { get; init; } = 10;

    public TimeSpan RepeatFlashCardPeriod { get; init; } = TimeSpan.FromDays(7);

    public int RepeatFlashCardRateFactor = 1;
}
