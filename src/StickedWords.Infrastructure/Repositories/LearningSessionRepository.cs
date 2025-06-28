using Microsoft.EntityFrameworkCore;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Infrastructure.Repositories;

internal class LearningSessionRepository : ILearningSessionRepository
{
    private readonly StickedWordsDbContext _context;

    public LearningSessionRepository(StickedWordsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<LearningSession?> GetActive(CancellationToken cancellationToken)
    {
        return await _context.LearningSessions
            .Include(x => x.Stages)
            .Include(x => x.FlashCards)
            .FirstOrDefaultAsync(x => x.State == LearningSessionState.Active);
    }

    public async Task Add(LearningSession learningSession, CancellationToken cancellationToken)
    {
        _context.LearningSessions.Add(learningSession);
        await _context.SaveChangesAsync(cancellationToken); // TODO: коммит нужно делать за пределами репозитория
    }
}
