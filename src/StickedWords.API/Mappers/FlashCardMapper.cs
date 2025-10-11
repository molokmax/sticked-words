using StickedWords.API.Models;
using StickedWords.Application.Commands.FlashCards;
using StickedWords.Domain.Models;

namespace StickedWords.API.Mappers;

internal static class FlashCardMapper
{
    public static FlashCardShortDto ToShortDto(this FlashCard source) =>
        new()
        {
            Id = source.Id,
            Word = source.Word,
            Translation = source.Translation,
            Rate = source.Rate,
            RepeatAt = source.RepeatAt
        };

    public static CreateFlashCardCommand ToCommand(this CreateFlashCardRequestDto source) =>
        new()
        {
            Word = source.Word,
            Translation = source.Translation
        };
}
