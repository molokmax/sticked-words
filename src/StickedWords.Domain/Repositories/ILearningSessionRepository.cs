using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Specifications;

namespace StickedWords.Domain.Repositories;

public interface ILearningSessionRepository
{
    Task<LearningSession?> GetById(long sessionId, CancellationToken cancellationToken);

    Task<LearningSession?> GetActive(CancellationToken cancellationToken);

    Task<LearningSession?> GetActiveNotExpired(DateTimeOffset now, CancellationToken cancellationToken);

    Task<PageResult<LearningSession>> GetBySpecification(
        ISpecification<LearningSession> specification,
        bool includeTotal,
        CancellationToken cancellationToken);

    void Add(LearningSession learningSession);
}
