using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace StickedWords.Infrastructure.Postgres;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDb<TContext>(
        this IServiceCollection services, Action<NpgsqlDbContextOptionsBuilder>? configure)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var contextName = typeof(TContext).Name;
            var connectionString = configuration.GetConnectionString(contextName);

            options.UseNpgsql(connectionString, configure);
        });

        return services;
    }
}
