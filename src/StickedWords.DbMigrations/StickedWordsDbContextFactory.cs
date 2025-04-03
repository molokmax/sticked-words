using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using StickedWords.Infrastructure;

namespace StickedWords.DbMigrations;

public class StickedWordsDbContextFactory : IDesignTimeDbContextFactory<StickedWordsDbContext>
{
    public StickedWordsDbContext CreateDbContext(string[] args)
    {
        var configuration = GetAppConfiguration(args);
        var connectionString = SqliteConnectionStringProvider.Get(configuration, nameof(StickedWordsDbContext));
        var optionsBuilder = new DbContextOptionsBuilder<StickedWordsDbContext>();
        optionsBuilder.UseSqlite(connectionString, ConfigureSqliteOptions);

        return new(optionsBuilder.Options);
    }

    private static IConfiguration GetAppConfiguration(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddCommandLine(args);

        return builder.Build();
    }

    private static void ConfigureSqliteOptions(SqliteDbContextOptionsBuilder options) =>
        options.MigrationsAssembly(AssemblyReference.Assembly);
}
