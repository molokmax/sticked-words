using Microsoft.EntityFrameworkCore;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Repositories;
using StickedWords.Domain.Specifications;

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

    public async Task<PageResult<FlashCard>> GetBySpecification(
        ISpecification<FlashCard> specification,
        bool includeTotal,
        CancellationToken cancellationToken)
    {
        var query = _context.FlashCards.AsQueryable();

        int? total = includeTotal
            ? await GetTotal(query, specification, cancellationToken)
            : null;

        var data = await query
            .GetQuery(specification)
            .ToListAsync(cancellationToken);

        return new(data, total);
    }

    public void Add(FlashCard flashCard)
    {
        _context.FlashCards.Add(flashCard);
    }

    private static async Task<int> GetTotal(
        IQueryable<FlashCard> query,
        ISpecification<FlashCard> specification,
        CancellationToken cancellationToken)
    {
        return await query.GetFilteredQuery(specification).CountAsync(cancellationToken);
    }
}
