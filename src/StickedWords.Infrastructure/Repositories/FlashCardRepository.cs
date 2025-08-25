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

    public async ValueTask<FlashCard?> GetById(long id, CancellationToken cancellationToken)
    {
        return await _context.FlashCards.FindAsync(id, cancellationToken);
    }

    public async Task<PageResult<FlashCard>> GetByQuery(PageQuery pageQuery, CancellationToken cancellationToken)
    {
        var query = _context.FlashCards.AsQueryable();

        var total = await GetTotal(query, pageQuery, cancellationToken);
        query = query.OrderByDescending(x => x.Rate).Skip(pageQuery.Skip);
        if (pageQuery.Take is not null)
        {
            query = query.Take(pageQuery.Take.Value);
        }

        var data = await query.ToListAsync(cancellationToken);

        return new(data, total);
    }

    public void Add(FlashCard flashCard)
    {
        _context.FlashCards.Add(flashCard);
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
