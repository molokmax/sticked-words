namespace StickedWords.API;

public static class CustomClaimTypes
{
    internal const string ClaimTypeNamespace = "sticked-words";

    public const string Id = ClaimTypeNamespace + "/id";

    public const string Login = ClaimTypeNamespace + "/login";

    public const string Surname = ClaimTypeNamespace + "/surname";

    public const string GivenName = ClaimTypeNamespace + "/givenName";

    public const string Email = ClaimTypeNamespace + "/email";

    public const string Provider = ClaimTypeNamespace + "/provider";
}
