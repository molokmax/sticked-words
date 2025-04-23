using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StickedWords.Infrastructure.Sqlite;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqliteDb<TContext>(
        this IServiceCollection services, Action<SqliteDbContextOptionsBuilder>? configure)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var contextName = typeof(TContext).Name;
            var connectionString = SqliteConnectionStringProvider.Get(configuration, contextName);

            options.UseSqlite(connectionString, configure);
        });

        return services;
    }
}
