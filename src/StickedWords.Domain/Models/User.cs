namespace StickedWords.Domain.Models;

public record User
{
    private User() { }

    public long Id { get; private set; }

    public string? YandexId { get; private set; }

    public static User Create(string yandexId)
    {
        return new()
        {
            YandexId = yandexId
        };
    }
}
