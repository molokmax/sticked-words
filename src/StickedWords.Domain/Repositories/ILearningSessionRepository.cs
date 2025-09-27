using StickedWords.Domain.Models;

namespace StickedWords.Domain.Repositories;

public interface ILearningSessionRepository
{
    Task<LearningSession?> GetById(long sessionId, CancellationToken cancellationToken);

    Task<LearningSession?> GetActive(CancellationToken cancellationToken);

    void Add(LearningSession learningSession);
}
