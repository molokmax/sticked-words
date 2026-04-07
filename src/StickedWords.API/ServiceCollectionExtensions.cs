using System.Security.Claims;
using System.Text.Json.Serialization;
using AspNet.Security.OAuth.Yandex;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using StickedWords.API.Endpoints;
using StickedWords.Application;
using StickedWords.Background;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Infrastructure;
using StickedWords.Infrastructure.Postgres;
using StickedWords.Infrastructure.Sqlite;

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
                builder.AddSqliteDb<StickedWordsDbContext>(opts => opts.MigrationsAssembly(DbMigrations.Sqlite.AssemblyReference.Assembly));
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

    public static IHostApplicationBuilder AddAuth(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = YandexAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
            })
            .AddYandex(options =>
            {
                options.ClientId = builder.Configuration.GetValue("Auth:Yandex:ClientId", string.Empty);
                options.ClientSecret = builder.Configuration.GetValue("Auth:Yandex:Secret", string.Empty);
                options.CallbackPath = "/api/auth/signin-yandex";
                options.SaveTokens = true;
                options.Scope.Add("login:email");
                options.Scope.Add("login:info");

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        if (context.Identity is not null)
                        {
                            MapClaim(context.Identity, ClaimTypes.NameIdentifier, CustomClaimTypes.Id);
                            MapClaim(context.Identity, ClaimTypes.Name, CustomClaimTypes.Login);
                            MapClaim(context.Identity, ClaimTypes.Surname, CustomClaimTypes.Surname);
                            MapClaim(context.Identity, ClaimTypes.GivenName, CustomClaimTypes.GivenName);
                            MapClaim(context.Identity, ClaimTypes.Email, CustomClaimTypes.Email);
                            context.Identity.AddClaim(new Claim(CustomClaimTypes.Provider, "Yandex"));
                        }
                    }
                };

            });

        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IUserInfoProvider, UserInfoProvider>();

        return builder;
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
        app.RegisterAuthEndpoints();
        app.RegisterFlashCardEndpoints();
        app.RegisterLearningSessionEndpoints();
        app.RegisterTranslateToNativeExerciseEndpoints();
        app.RegisterTranslateToForeignExerciseEndpoints();
        app.RegisterImageEndpoints();

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

    private static ClaimsIdentity MapClaim(
        ClaimsIdentity identity,
        string sourceClaimType,
        string targetClaimType,
        bool removeSourceClaim = true)
    {
        var claim = identity.FindFirst(sourceClaimType);
        if (claim is not null)
        {
            if (removeSourceClaim)
            {
                identity.RemoveClaim(claim);
            }
            identity.AddClaim(new Claim(targetClaimType, claim.Value));
        }

        return identity;
    }
}
