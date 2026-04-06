using System.Security.Claims;
using AspNet.Security.OAuth.Yandex;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using StickedWords.API.Models;

namespace StickedWords.API.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder RegisterAuthEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/auth");

        group.MapGet("/me", UserInfo);
        group.MapGet("/login-yandex", LoginYandex);
        group.MapDelete("/logout", Logout).RequireAuthorization();

        return builder;
    }

    private static IResult LoginYandex(string returnUrl = "/")
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = returnUrl,
            IsPersistent = true,
            Items = { { "returnUrl", returnUrl } }
        };

        return Results.Challenge(props, [YandexAuthenticationDefaults.AuthenticationScheme]);
    }

    private static UserInfoDto? UserInfo(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var result = new UserInfoDto
        {
            Id = 0,
            Login = context.User.FindFirstValue(CustomClaimTypes.Login) ?? string.Empty,
            Surname = context.User.FindFirstValue(CustomClaimTypes.Surname),
            GivenName = context.User.FindFirstValue(CustomClaimTypes.GivenName),
            Email = context.User.FindFirstValue(CustomClaimTypes.Email),
            ExternalId = context.User.FindFirstValue(CustomClaimTypes.Id) ?? string.Empty,
            AuthProvider = context.User.FindFirstValue(CustomClaimTypes.Provider) ?? string.Empty
        };

        return result;
    }

    private static async Task Logout(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
