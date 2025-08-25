using Microsoft.Extensions.DependencyInjection;
using StickedWords.Domain;
using StickedWords.Domain.Repositories;
using StickedWords.Infrastructure.Repositories;

namespace StickedWords.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddTransient<IFlashCardRepository, FlashCardRepository>();
        services.AddTransient<ILearningSessionRepository, LearningSessionRepository>();
        services.AddTransient<IGuessRepository, GuessRepository>();

        return services;
    }
}
