using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;

namespace StickedWords.Application.Services;

internal sealed class AccessPolicy
{
    public void Check(User user, FlashCard flashCard)
    {
        if (flashCard.UserId != user.Id)
        {
            throw new AccessToFlashCardDeniedException(flashCard.Id, user.Id);
        }
    }
}
