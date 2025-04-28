using Microsoft.EntityFrameworkCore;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Repositories;

namespace StickedWords.Infrastructure.Repositories;

internal class FlashCardRepository : IFlashCardRepository
{
    private readonly StickedWordsDbContext _context;

    public FlashCardRepository(StickedWordsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<PageResult<FlashCard>> GetByQuery(PageQuery pageQuery, CancellationToken cancellationToken)
    {
        var query = _context.FlashCards.AsQueryable();

        var total = await GetTotal(query, pageQuery, cancellationToken);
        query = query
            .OrderByDescending(x => x.CreatedAt) // TODO: это не работает в sqlite (сортировка по дате). в любом случае нужно будет модифицировать сортировку. первыми должны отображаться слова, которые нужно повторить
            .Skip(pageQuery.Skip);
        if (pageQuery.Take is not null)
        {
            query = query.Take(pageQuery.Take.Value);
        }

        var data = await query.ToListAsync(cancellationToken);

        return new(data, total);
    }

    public async Task Add(FlashCard flashCard, CancellationToken cancellationToken)
    {
        _context.FlashCards.Add(flashCard);
        await _context.SaveChangesAsync(cancellationToken); // TODO: коммит нужно делать за пределами репозитория
    }

    private static async ValueTask<int?> GetTotal(
        IQueryable<FlashCard> query,
        PageQuery pageQuery,
        CancellationToken cancellationToken)
    {
        if (!pageQuery.IncludeTotal)
        {
            return null;
        }

        return await query.CountAsync(cancellationToken);
    }
}
