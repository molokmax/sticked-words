using Microsoft.EntityFrameworkCore;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly StickedWordsDbContext _context;

    public UserRepository(StickedWordsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async ValueTask<User?> GetById(long id, CancellationToken cancellationToken)
    {
        return await _context.Users.FindAsync(id, cancellationToken);
    }

    public async Task<User?> GetByYandexId(string yandexId, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.YandexId == yandexId, cancellationToken);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
    }
}
