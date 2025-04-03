using StickedWords.API.Models;
using StickedWords.Domain.Models;

namespace StickedWords.API.Mappers;

internal static class FlashCardMapper
{
    public static FlashCardShortDto ToShortDto(this FlashCard source) =>
        new()
        {
            Id = source.Id,
            Word = source.Word,
            Translation = source.Translation
        };
}
