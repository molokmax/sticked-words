using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;

namespace StickedWords.Domain.Repositories;

public interface IFlashCardRepository
{
    Task<PageResult<FlashCard>> GetByQuery(PageQuery pageQuery, CancellationToken cancellationToken);
}
