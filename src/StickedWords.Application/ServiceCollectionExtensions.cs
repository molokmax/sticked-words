using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StickedWords.Domain;

namespace StickedWords.Application;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<LearningSessionOptions>(builder.Configuration.GetSection(LearningSessionOptions.SectionName)); // TODO: добавить отдельный проект с базовыми функциями
        builder.Services.AddMediatR(opts => opts.RegisterServicesFromAssembly(AssemblyReference.Assembly));

        return builder;
    }
}
