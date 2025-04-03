using StickedWords.Application;
using StickedWords.Infrastructure;

namespace StickedWords.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddApplication();

        return services;
    }
}
