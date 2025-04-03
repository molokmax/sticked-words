namespace StickedWords.Domain.Models.Paging;

public record PageQuery
{
    public int Skip { get; init; } = 0;

    public int? Take { get; init; } = 50;

    public bool IncludeTotal { get; init; } = true;
}
