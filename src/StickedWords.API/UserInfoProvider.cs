using System.Security.Claims;
using StickedWords.Domain;
using StickedWords.Domain.Models;

namespace StickedWords.API;

internal sealed class UserInfoProvider : IUserInfoProvider
{
    private readonly IHttpContextAccessor _httpAccessor;

    public UserInfoProvider(IHttpContextAccessor httpAccessor)
    {
        _httpAccessor = httpAccessor;
    }

    public UserInfo? Get()
    {
        var user = _httpAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        return new()
        {
            Login = user.FindFirstValue(CustomClaimTypes.Login) ?? string.Empty,
            Surname = user.FindFirstValue(CustomClaimTypes.Surname),
            GivenName = user.FindFirstValue(CustomClaimTypes.GivenName),
            Email = user.FindFirstValue(CustomClaimTypes.Email),
            ExternalId = user.FindFirstValue(CustomClaimTypes.Id) ?? string.Empty,
            AuthProvider = user.FindFirstValue(CustomClaimTypes.Provider) ?? string.Empty
        };
    }
}
