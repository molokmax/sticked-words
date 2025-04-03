using StickedWords.API.Models;
using StickedWords.Domain.Models.Paging;

namespace StickedWords.API.Mappers;

internal static class PageMapper
{
    public static PageQuery ToDomain(this PageQueryDto source) =>
        new()
        {
            Skip = source.Skip,
            Take = source.Take,
            IncludeTotal = source.IncludeTotal
        };

    public static PageResultDto<TDto> ToDto<TDomain, TDto>(
        this PageResult<TDomain> source,
        Func<TDomain, TDto> map)
    {
        return new()
        {
            Data = source.Data.Select(map).ToArray(),
            Total = source.Total
        };
    }
}
