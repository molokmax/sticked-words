using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StickedWords.Application.Services;
using StickedWords.Domain;
using StickedWords.Shared;

namespace StickedWords.Application;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.ConfigureFromConfiguration<LearningSessionOptions>();
        builder.Services.AddMediatR(opts => opts.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        builder.Services.AddTransient<UserService>();
        builder.Services.AddTransient<AccessPolicy>();

        return builder;
    }
}
