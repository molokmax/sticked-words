using StickedWords.Domain.Models;

namespace StickedWords.Domain;

public interface IUserInfoProvider
{
    UserInfo? Get();
}
