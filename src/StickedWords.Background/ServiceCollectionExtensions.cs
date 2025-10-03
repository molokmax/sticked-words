using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using StickedWords.Infrastructure;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.DependencyInjection;

namespace StickedWords.Background;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddBackground(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTickerQ(options =>
        {
            options.SetMaxConcurrency(10);
            options.AddOperationalStore<StickedWordsDbContext>(opts =>
            {
                opts.UseModelCustomizerForMigrations();
            });
            options.AddDashboard(opts =>
            {
                opts.BasePath = "/tickerq";
            });
        });

        return builder;
    }

    public static IApplicationBuilder UseBackground(this IApplicationBuilder app)
    {
        app.UseTickerQ();

        return app;
    }
}
