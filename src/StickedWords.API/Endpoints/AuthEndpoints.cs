using System.Security.Claims;
using AspNet.Security.OAuth.Yandex;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.Domain;

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

    private static UserInfoDto? UserInfo(IUserInfoProvider userProvider)
    {
        return userProvider.Get()?.ToDto();
    }

    private static async Task Logout(HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
