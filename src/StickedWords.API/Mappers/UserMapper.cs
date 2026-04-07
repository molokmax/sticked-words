using StickedWords.API.Models;
using StickedWords.Domain.Models;

namespace StickedWords.API.Mappers;

internal static class UserMapper
{
    public static UserInfoDto ToDto(this UserInfo source) =>
        new()
        {
            Login = source.Login,
            Surname = source.Surname,
            GivenName = source.GivenName,
            Email = source.Email,
            ExternalId = source.ExternalId,
            AuthProvider = source.AuthProvider
        };
}
