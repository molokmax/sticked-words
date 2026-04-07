using StickedWords.Domain.Models;

namespace StickedWords.Domain.Repositories;

public interface IUserRepository
{
    ValueTask<User?> GetById(long id, CancellationToken cancellationToken);

    Task<User?> GetByYandexId(string yandexId, CancellationToken cancellationToken);

    void Add(User user);
}
