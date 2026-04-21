using StickedWords.Domain;
using StickedWords.Domain.Models;
using StickedWords.Domain.Specifications;

namespace StickedWords.Application.Specifications;

internal sealed class FlashCardsToChooseSpecification : Specification<FlashCard>
{
    public FlashCardsToChooseSpecification(User user, FlashCard excludeFlashCard, int take = 3)
    {
        Criteria = x => x.UserId == user.Id && x.Id != excludeFlashCard.Id;
        AddOrder(x => x.Rate);
        SetPaging(0, take);
    }
}
