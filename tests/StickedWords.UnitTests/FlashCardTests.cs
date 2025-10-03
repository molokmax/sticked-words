using Microsoft.Extensions.Time.Testing;
using StickedWords.Domain;
using StickedWords.Domain.Models;

namespace StickedWords.UnitTests;

public class FlashCardTests
{
    [Test]
    public void RefreshRate_RateIs100_RepeatAtCalculated()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var timeProvider = GetTimeProvider(now);
        var options = GetOptions(7);
        var flashCard = GetFlashCard();
        flashCard.UpdateBaseRate(100, options, timeProvider);

        // Act
        flashCard.RefreshRate(options, timeProvider);

        // Assert
        Assert.That(flashCard.RepeatAt, Is.EqualTo(now.AddDays(7)));
    }

    [Test]
    public void RefreshRate_RateIs50_RepeatAtCalculated()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var timeProvider = GetTimeProvider(now);
        var options = GetOptions(7);
        var flashCard = GetFlashCard();
        flashCard.UpdateBaseRate(50, options, timeProvider);

        // Act
        flashCard.RefreshRate(options, timeProvider);

        // Assert
        Assert.That(flashCard.RepeatAt, Is.EqualTo(now.AddDays(4)));
    }

    [Test]
    public void RefreshRate_RateIs20_RepeatAtCalculated()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var timeProvider = GetTimeProvider(now);
        var options = GetOptions(7);
        var flashCard = GetFlashCard();
        flashCard.UpdateBaseRate(20, options, timeProvider);

        // Act
        flashCard.RefreshRate(options, timeProvider);

        // Assert
        Assert.That(flashCard.RepeatAt, Is.EqualTo(now.AddDays(2)));
    }

    private static FlashCard GetFlashCard() =>
        FlashCard.Create("word1", "слово1", GetTimeProvider(DateTimeOffset.UtcNow));

    private static LearningSessionOptions GetOptions(int period)
    {
        return new()
        {
            RepeatFlashCardPeriod = TimeSpan.FromDays(period)
        };
    }

    private static TimeProvider GetTimeProvider(DateTimeOffset now) =>
        new FakeTimeProvider(now);
}
