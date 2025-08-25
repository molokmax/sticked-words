using StickedWords.Domain.Models;

namespace StickedWords.Domain.Repositories;

public interface IGuessRepository
{
    Task<IReadOnlyCollection<Guess>> Get(long sessionStageId, CancellationToken cancellationToken);

    void Add(Guess guess);
}
