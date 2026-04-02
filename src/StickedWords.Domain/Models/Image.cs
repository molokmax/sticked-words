namespace StickedWords.Domain.Models;

public record Image
{
    public long Id { get; init; }

    public required string Base64Data { get; init; }

    public DateTimeOffset UploadedAt { get; init; }

    public static Image Create(string base64data, TimeProvider timeProvider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(base64data);

        return new()
        {
            Base64Data = base64data,
            UploadedAt = timeProvider.GetUtcNow()
        };
    }
}
