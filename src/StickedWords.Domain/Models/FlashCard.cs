namespace StickedWords.Domain.Models;

public class FlashCard
{
    public long Id { get; init; }

    public required string Word { get; init; }

    public required string Translation { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}
