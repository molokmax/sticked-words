using Microsoft.EntityFrameworkCore;
using StickedWords.Domain.Models;
using StickedWords.Infrastructure;

namespace StickedWords.Domain.Repositories;

internal sealed class GuessRepository : IGuessRepository
{
    private readonly StickedWordsDbContext _context;

    public GuessRepository(StickedWordsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<IReadOnlyCollection<Guess>> Get(long sessionStageId, CancellationToken cancellationToken)
    {
        return await _context.Guesses.Where(x => x.SessionStageId == sessionStageId).ToListAsync(cancellationToken);
    }

    public void Add(Guess guess)
    {
        _context.Guesses.Add(guess);
    }
}
