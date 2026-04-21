using StickedWords.API.Models.Exercises;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.API.Mappers;

public static class TranslateGuessMapper
{
    public static GuessResultDto ToDto(this GuessResult source)
    {
        return new()
        {
            Result = source.Result,
            CorrectTranslation = source.CorrectTranslation,
            IsExpired = source.IsExpired
        };
    }
}
