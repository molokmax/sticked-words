using Microsoft.EntityFrameworkCore;
using StickedWords.Domain.Models;
using TickerQ.EntityFrameworkCore.Configurations;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TimeTickerConfigurations());
        modelBuilder.ApplyConfiguration(new CronTickerConfigurations());
        modelBuilder.ApplyConfiguration(new CronTickerOccurrenceConfigurations());
    }
}
