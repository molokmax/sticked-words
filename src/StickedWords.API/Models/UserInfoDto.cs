namespace StickedWords.API.Models;

public record UserInfoDto
{
    public long Id { get; init; }

    public required string Login { get; init; }

    public required string? Surname { get; init; }

    public required string? GivenName { get; init; }

    public required string? Email { get; init; }

    public required string ExternalId { get; init; }

    public required string AuthProvider { get; init; }
}
