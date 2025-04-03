namespace StickedWords.API.Models;

public record PageResultDto<T>
{
    public required IReadOnlyCollection<T> Data { get; init; }

    public required int? Total { get; init; }
}
