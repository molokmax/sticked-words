using System.Diagnostics.CodeAnalysis;

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

    public int Rate { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public static FlashCard Create(string word, string translation)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(word);
        ArgumentException.ThrowIfNullOrWhiteSpace(translation);

        return new()
        {
            Word = word,
            Translation = translation,
            CreatedAt = DateTimeOffset.UtcNow,
            Rate = 1000
        };
    }
}
