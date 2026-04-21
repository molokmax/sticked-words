namespace StickedWords.Domain.Models;

public interface IExerciseResult
{
    Verdict Result { get; }

    bool IsExpired { get; }
}
