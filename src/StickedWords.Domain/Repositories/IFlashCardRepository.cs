using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Specifications;

namespace StickedWords.Domain.Repositories;

public interface IFlashCardRepository
{
    ValueTask<FlashCard?> GetById(long id, CancellationToken cancellationToken);

    Task<PageResult<FlashCard>> GetBySpecification(
        ISpecification<FlashCard> specification,
        bool includeTotal,
        CancellationToken cancellationToken);

    void Add(FlashCard flashCard);
}
