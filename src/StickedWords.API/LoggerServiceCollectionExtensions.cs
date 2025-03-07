using Serilog;

namespace StickedWords.API;

public static class LoggerServiceCollectionExtensions
{
    public static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        builder.Services.AddSerilog(opts => opts.ReadFrom.Configuration(builder.Configuration));

        return builder;
    }
}
