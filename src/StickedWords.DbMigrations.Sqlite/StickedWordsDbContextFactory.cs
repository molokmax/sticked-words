using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using StickedWords.Infrastructure;
using StickedWords.Infrastructure.Sqlite;

namespace StickedWords.DbMigrations.Sqlite;

public class StickedWordsDbContextFactory : IDesignTimeDbContextFactory<StickedWordsDbContext>
{
    public StickedWordsDbContext CreateDbContext(string[] args)
    {
        var configuration = GetAppConfiguration(args);
        var optionsBuilder = new DbContextOptionsBuilder<StickedWordsDbContext>();
        ConfigureSqlite(optionsBuilder, configuration);

        return new(optionsBuilder.Options);
    }

    private static IConfiguration GetAppConfiguration(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args);

        return builder.Build();
    }

    private static void ConfigureSqlite(
        DbContextOptionsBuilder<StickedWordsDbContext> optionsBuilder, IConfiguration configuration)
    {
        var connectionString = SqliteConnectionStringProvider.Get(configuration, nameof(StickedWordsDbContext));
        optionsBuilder.UseSqlite(connectionString, opts => opts.MigrationsAssembly(AssemblyReference.Assembly));
    }
}
