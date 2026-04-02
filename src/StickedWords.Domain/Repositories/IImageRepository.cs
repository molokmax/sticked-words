using StickedWords.Domain.Models;

namespace StickedWords.Domain.Repositories;

public interface IImageRepository
{
    ValueTask<Image?> GetById(long id, CancellationToken cancellationToken);

    void Add(Image image);

    void Delete(Image image);
}
