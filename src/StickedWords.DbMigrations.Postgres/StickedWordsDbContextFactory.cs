using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using StickedWords.Infrastructure;

namespace StickedWords.DbMigrations.Postgres;

public class StickedWordsDbContextFactory : IDesignTimeDbContextFactory<StickedWordsDbContext>
{
    public StickedWordsDbContext CreateDbContext(string[] args)
    {
        var configuration = GetAppConfiguration(args);
        var optionsBuilder = new DbContextOptionsBuilder<StickedWordsDbContext>();
        ConfigurePostgres(optionsBuilder, configuration);

        return new(optionsBuilder.Options);
    }

    private static IConfiguration GetAppConfiguration(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddCommandLine(args);

        return builder.Build();
    }

    private static void ConfigurePostgres(
        DbContextOptionsBuilder<StickedWordsDbContext> optionsBuilder, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(StickedWordsDbContext));
        optionsBuilder.UseNpgsql(connectionString, opts => opts.MigrationsAssembly(AssemblyReference.Assembly));
    }
}
