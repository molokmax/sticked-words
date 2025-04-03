namespace StickedWords.Domain.Models.Paging;

public record PageResult<T>
{
    public PageResult() { }

    public PageResult(IReadOnlyCollection<T> data, int? total = null)
    {
        Data = data;
        Total = total;
    }

    public IReadOnlyCollection<T> Data { get; init; } = [];

    public int? Total { get; init; }
}
