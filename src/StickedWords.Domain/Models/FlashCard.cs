using System.Diagnostics.CodeAnalysis;
using StickedWords.Domain.Exceptions;

namespace StickedWords.Domain.Models;

public class FlashCard
{
    private FlashCard() { }

    public long Id { get; private set; }

    /// <summary>
    /// Foreign word.
    /// </summary>
    public string Word { get; private set; } = string.Empty;

    /// <summary>
    /// Translation to native language.
    /// </summary>
    public string Translation { get; private set; } = string.Empty;

    public int BaseRate { get; private set; }

    public int Rate { get; private set; }

    public DateTimeOffset RepeatAt { get; private set; }

    public long RepeatAtUnixTime { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public void UpdateBaseRate(int rate, LearningSessionOptions options, TimeProvider timeProvider)
    {
        if (rate < 0 || rate > 100)
        {
            throw new RateIsNotValidException(rate);
        }

        BaseRate = rate;
        Rate = rate;

        var repeatAfterDays = options.RepeatFlashCardPeriod.TotalDays * (rate / 100d);
        RepeatAt = timeProvider.GetUtcNow().AddDays(Math.Ceiling(repeatAfterDays));
        RepeatAtUnixTime = RepeatAt.ToUnixTime();
    }

    public void RefreshRate(LearningSessionOptions options, TimeProvider timeProvider)
    {
        if (Rate <= 0)
        {
            return;
        }

        var daySpan = timeProvider.GetUtcNow() - RepeatAt;
        if (daySpan.TotalDays <= 0)
        {
            return;
        }

        var rateDecrease = daySpan.TotalDays / options.RepeatFlashCardRateFactor;
        Rate = BaseRate - Convert.ToInt32(Math.Floor(rateDecrease));
    }

    public static FlashCard Create(string word, string translation, TimeProvider timeProvider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(word);
        ArgumentException.ThrowIfNullOrWhiteSpace(translation);

        return new()
        {
            Word = word,
            Translation = translation,
            CreatedAt = timeProvider.GetUtcNow(),
            RepeatAt = timeProvider.GetUtcNow(),
            Rate = 0
        };
    }
}
