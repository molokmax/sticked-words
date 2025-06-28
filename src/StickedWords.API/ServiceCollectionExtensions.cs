using StickedWords.Application;
using StickedWords.Infrastructure;
using StickedWords.Infrastructure.Sqlite;
using StickedWords.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using StickedWords.API.Endpoints;
using System.Text.Json.Serialization;

namespace StickedWords.API;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSqliteDb<StickedWordsDbContext>(opts => opts.MigrationsAssembly(DbMigrations.Sqlite.AssemblyReference.Assembly));
        // services.AddPostgresDb<StickedWordsDbContext>(opts => opts.MigrationsAssembly(DbMigrations.Postgres.AssemblyReference.Assembly));
        builder.Services.AddRepositories();
        builder.AddApplication();

        return builder;
    }

    public static IServiceCollection ConfigureHttp(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }

    public static IApplicationBuilder RegisterSpa(this IApplicationBuilder app)
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseSpa(opts => opts.Options.DefaultPageStaticFileOptions = new());

        return app;
    }

    public static IEndpointRouteBuilder RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app.RegisterFlashCardEndpoints();
        app.RegisterLearningSessionEndpoints();
        app.RegisterTranslateToNativeExerciseEndpoints();
        app.RegisterTranslateToForeignExerciseEndpoints();

        return app;
    }

    public static IServiceProvider ApplyMigrations(this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StickedWordsDbContext>();
            context.Database.Migrate();
        }

        return services;
    }
}
