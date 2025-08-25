using StickedWords.Domain;

namespace StickedWords.Infrastructure;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly StickedWordsDbContext _dbContext;

    public UnitOfWork(StickedWordsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChanges(CancellationToken cancellationToken) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}
