using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StickedWords.Shared;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder ConfigureFromConfiguration<TOptions>(this IHostApplicationBuilder builder)
        where TOptions : class, IConfigurationOptions
    {
        var section = builder.Configuration.GetSection(TOptions.SectionName);
        builder.Services.Configure<TOptions>(section);

        return builder;
    }
}
