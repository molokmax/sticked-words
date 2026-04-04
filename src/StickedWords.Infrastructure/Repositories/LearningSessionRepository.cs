using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using StickedWords.Domain;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Repositories;
using StickedWords.Domain.Specifications;

namespace StickedWords.Infrastructure.Repositories;

internal class LearningSessionRepository : ILearningSessionRepository
{
    private readonly StickedWordsDbContext _context;

    public LearningSessionRepository(StickedWordsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<LearningSession?> GetById(long sessionId, CancellationToken cancellationToken)
    {
        return await WithIncludes(_context.LearningSessions)
            .FirstOrDefaultAsync(x => x.Id == sessionId, cancellationToken);
    }

    public async Task<LearningSession?> GetActive(CancellationToken cancellationToken)
    {
        return await WithIncludes(_context.LearningSessions)
            .FirstOrDefaultAsync(x => x.State == LearningSessionState.Active);
    }

    public async Task<LearningSession?> GetActiveNotExpired(DateTimeOffset now, CancellationToken cancellationToken)
    {
        return await WithIncludes(_context.LearningSessions)
            .FirstOrDefaultAsync(x => x.State == LearningSessionState.Active && x.ExpiringAtUnixTime > now.ToUnixTime());
    }

    public async Task<PageResult<LearningSession>> GetBySpecification(
        ISpecification<LearningSession> specification,
        bool includeTotal,
        CancellationToken cancellationToken)
    {
        var query = _context.LearningSessions.AsQueryable();

        int? total = includeTotal
            ? await GetTotal(query, specification, cancellationToken)
            : null;

        var data = await WithIncludes(query)
            .GetQuery(specification)
            .ToListAsync(cancellationToken);

        return new(data, total);
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

    private static IQueryable<LearningSession> WithIncludes(
        IQueryable<LearningSession> query)
    {
        return query.Include(x => x.Stages)
            .ThenInclude(x => x.Guesses)
            .Include(x => x.FlashCards)
            .ThenInclude(x => x.FlashCard);
    }

    private static async Task<int> GetTotal(
        IQueryable<LearningSession> query,
        ISpecification<LearningSession> specification,
        CancellationToken cancellationToken)
    {
        return await query.GetFilteredQuery(specification).CountAsync(cancellationToken);
    }
}
