using StickedWords.Domain;
using StickedWords.Domain.Models;
using StickedWords.Domain.Specifications;

namespace StickedWords.Application.Specifications;

internal sealed class LearningSessionsToExpireSpecification : Specification<LearningSession>
{
    public LearningSessionsToExpireSpecification(DateTimeOffset now, int skip = 0, int? take = 100)
    {
        Criteria = x => x.State == LearningSessionState.Active && x.ExpiringAtUnixTime < now.ToUnixTime();
        AddOrder(x => x.ExpiringAtUnixTime);
        SetPaging(skip, take);
    }
}
