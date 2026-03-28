namespace StickedWords.Domain.Models;

public interface ISoftDeletable
{
    bool IsDeleted { get; }

    DateTimeOffset? DeletedAt { get; }
}
