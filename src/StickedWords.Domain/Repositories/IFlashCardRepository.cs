using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;

namespace StickedWords.Domain.Repositories;

public interface IFlashCardRepository
{
    ValueTask<FlashCard?> GetById(long id, CancellationToken cancellationToken);

    Task<PageResult<FlashCard>> GetByQuery(PageQuery pageQuery, CancellationToken cancellationToken);

    Task Add(FlashCard flashCard, CancellationToken cancellationToken);
}
