using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using StickedWords.Infrastructure;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.Customizer;
using TickerQ.EntityFrameworkCore.DependencyInjection;

namespace StickedWords.Background;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddBackground(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTickerQ(options =>
        {
            options.AddOperationalStore(efOptions =>
            {
                efOptions.UseApplicationDbContext<StickedWordsDbContext>(ConfigurationType.IgnoreModelCustomizer);
            });
            options.AddDashboard(opts =>
            {
                opts.SetBasePath("/tickerq");
            });
        });

        return builder;
    }

    public static IHost UseBackground(this IHost app)
    {
        app.UseTickerQ();

        return app;
    }
}
