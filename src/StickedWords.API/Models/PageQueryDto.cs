namespace StickedWords.API.Models;

public record PageQueryDto
{
    public const int SkipDefaultValue = 0;
    public const bool IncludeTotalDefaultValue = true;

    public int Skip { get; init; } = SkipDefaultValue;

    public int? Take { get; init; }

    public bool IncludeTotal { get; init; } = IncludeTotalDefaultValue;

    public static ValueTask<PageQueryDto?> BindAsync(HttpContext context)
    {
        var skip = int.TryParse(context.Request.Query["skip"], out var s) ? s : SkipDefaultValue;
        var take = int.TryParse(context.Request.Query["take"], out var t) ? t : (int?)null;
        bool includeTotal = bool.TryParse(context.Request.Query["includeTotal"], out var i) ? i : IncludeTotalDefaultValue;

        var result = new PageQueryDto
        {
            Skip = skip,
            Take = take,
            IncludeTotal = includeTotal
        };

        return ValueTask.FromResult<PageQueryDto?>(result);
    }
}
