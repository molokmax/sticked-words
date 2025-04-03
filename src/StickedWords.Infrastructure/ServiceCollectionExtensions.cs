using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StickedWords.Domain.Repositories;
using StickedWords.Infrastructure.Repositories;

namespace StickedWords.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddDb();

        services.AddTransient<IFlashCardRepository, FlashCardRepository>();

        return services;
    }

    private static IServiceCollection AddDb(this IServiceCollection services)
    {
        services.AddDbContext<StickedWordsDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = SqliteConnectionStringProvider.Get(configuration, nameof(StickedWordsDbContext));

            options.UseSqlite(connectionString);
        });

        return services;
    }
}
