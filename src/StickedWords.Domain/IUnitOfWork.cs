namespace StickedWords.Domain;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken cancellationToken);
}
