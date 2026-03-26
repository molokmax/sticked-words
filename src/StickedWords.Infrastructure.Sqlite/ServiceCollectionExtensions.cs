using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace StickedWords.Infrastructure.Sqlite;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddSqliteDb<TContext>(
        this IHostApplicationBuilder builder, Action<SqliteDbContextOptionsBuilder>? configure)
        where TContext : DbContext
    {
        builder.AddSqliteDbContext<TContext>("sqlite", null, opts => opts.UseSqlite(configure));

        return builder;
    }
}
