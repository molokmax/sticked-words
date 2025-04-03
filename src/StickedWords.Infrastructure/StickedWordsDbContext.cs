using Microsoft.EntityFrameworkCore;
using StickedWords.Domain.Models;

namespace StickedWords.Infrastructure;

public sealed class StickedWordsDbContext : DbContext
{
    public DbSet<FlashCard> FlashCards { get; set; }

    public StickedWordsDbContext(DbContextOptions<StickedWordsDbContext> options)
        : base(options)
    {
    }
}
