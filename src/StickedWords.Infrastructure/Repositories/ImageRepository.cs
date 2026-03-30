using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Infrastructure.Repositories;

internal sealed class ImageRepository : IImageRepository
{
    private readonly StickedWordsDbContext _context;

    public ImageRepository(StickedWordsDbContext dbContext)
    {
        _context = dbContext;
    }

    public async ValueTask<Image?> GetById(long id, CancellationToken cancellationToken)
    {
        return await _context.Images.FindAsync(id, cancellationToken);
    }

    public void Add(Image image)
    {
        _context.Images.Add(image);
    }

    public void Delete(Image image)
    {
        _context.Images.Remove(image);
    }
}
