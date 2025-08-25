using StickedWords.API.Models.Exercises;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.API.Mappers;

public static class TranslateGuessMapper
{
    public static TranslateGuessResultDto ToDto(this TranslateGuessResult source)
    {
        return new()
        {
            Result = source.Result,
            CorrectTranslation = source.CorrectTranslation
        };
    }
}
