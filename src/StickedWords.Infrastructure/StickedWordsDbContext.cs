using Microsoft.EntityFrameworkCore;
using StickedWords.Domain.Models;

namespace StickedWords.Infrastructure;

public sealed class StickedWordsDbContext : DbContext
{
    public DbSet<FlashCard> FlashCards { get; set; }

    public DbSet<LearningSession> LearningSessions { get; set; }

    public DbSet<SessionStage> SessionStages { get; set; }

    public DbSet<SessionFlashCard> SessionFlashCards { get; set; }

    public DbSet<Guess> Guesses { get; set; }

    public StickedWordsDbContext(DbContextOptions<StickedWordsDbContext> options)
        : base(options)
    {
    }
}
