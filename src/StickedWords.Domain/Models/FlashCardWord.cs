namespace StickedWords.Domain.Models;

public sealed record FlashCardWord
{
    private FlashCardWord() { }

    public required string Word { get; init; }

    public bool Equals(FlashCardWord? other) =>
        string.Equals(Word, other?.Word, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() =>
        Word.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public static FlashCardWord Create(string word)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(word);

        return new() { Word = word.Trim() };
    }

    public static implicit operator FlashCardWord(string source) => Create(source);

    public static explicit operator string(FlashCardWord source) => source.Word;
}
