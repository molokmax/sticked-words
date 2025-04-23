using StickedWords.Application;
using StickedWords.DbMigrations.Postgres;
using StickedWords.Infrastructure;
using StickedWords.Infrastructure.Sqlite;
using StickedWords.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace StickedWords.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // services.AddSqliteDb<StickedWordsDbContext>(opts => opts.MigrationsAssembly(AssemblyReference.Assembly));
        services.AddPostgresDb<StickedWordsDbContext>(opts => opts.MigrationsAssembly(AssemblyReference.Assembly));
        services.AddRepositories();
        services.AddApplication();

        return services;
    }

    public static IServiceProvider ApplyMigrations(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StickedWordsDbContext>();
            context.Database.Migrate();
        }

        return services;
    }
}
