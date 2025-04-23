using Microsoft.Extensions.DependencyInjection;
using StickedWords.Domain.Repositories;
using StickedWords.Infrastructure.Repositories;

namespace StickedWords.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IFlashCardRepository, FlashCardRepository>();

        return services;
    }
}
