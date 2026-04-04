namespace StickedWords.Domain.Models;

public interface IExerciseResult
{
    GuessResult Result { get; }

    bool IsExpired { get; }
}
