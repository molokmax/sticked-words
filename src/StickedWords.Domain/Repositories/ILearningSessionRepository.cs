using StickedWords.Domain.Models;

namespace StickedWords.Domain.Repositories;

public interface ILearningSessionRepository
{
    Task<LearningSession?> GetActive(CancellationToken cancellationToken);

    Task Add(LearningSession learningSession, CancellationToken cancellationToken);
}
