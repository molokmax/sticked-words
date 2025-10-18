using StickedWords.Application;
using StickedWords.Infrastructure;
using StickedWords.Infrastructure.Sqlite;
using StickedWords.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using StickedWords.API.Endpoints;
using System.Text.Json.Serialization;
using StickedWords.Background;
using StickedWords.Domain.Exceptions;

namespace StickedWords.API;

public static class ServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);

        builder.AddDatabase();
        builder.Services.AddRepositories();
        builder.AddApplication();
        builder.AddBackground();

        return builder;
    }

    private static IHostApplicationBuilder AddDatabase(this IHostApplicationBuilder builder)
    {
        var dbProvider = builder.Configuration.GetValue("DbProvider", "SQLite");
        switch (dbProvider.ToUpper())
        {
            case Infrastructure.Sqlite.Consts.DbProviderName:
                builder.Services.AddSqliteDb<StickedWordsDbContext>(opts => opts.MigrationsAssembly(DbMigrations.Sqlite.AssemblyReference.Assembly));
                break;
            case Infrastructure.Postgres.Consts.DbProviderName:
                builder.Services.AddPostgresDb<StickedWordsDbContext>(opts => opts.MigrationsAssembly(DbMigrations.Postgres.AssemblyReference.Assembly));
                break;
            default:
                throw new DbProviderNotSupportedException(dbProvider);
        }

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
