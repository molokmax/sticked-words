using StickedWords.Domain;
using StickedWords.Domain.Models;
using StickedWords.Domain.Specifications;

namespace StickedWords.Application.Specifications;

internal sealed class FlashCardsToRepeatSpecification : Specification<FlashCard>
{
    public FlashCardsToRepeatSpecification(DateTimeOffset now, int skip = 0, int? take = 100)
    {
        Criteria = x => x.RepeatAtUnixTime < now.ToUnixTime();
        AddOrder(x => x.Rate);
        SetPaging(skip, take);
    }
}
