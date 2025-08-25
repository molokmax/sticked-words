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
            .ThenInclude(x => x.Guesses)
            .Include(x => x.FlashCards)
            .ThenInclude(x => x.FlashCard)
            .FirstOrDefaultAsync(x => x.State == LearningSessionState.Active);
    }

    public void Add(LearningSession learningSession)
    {
        _context.LearningSessions.Add(learningSession);
    }

    public void Update(LearningSession learningSession)
    {
        _context.Attach(learningSession);
        _context.Entry(learningSession).State = EntityState.Modified;
    }
}
