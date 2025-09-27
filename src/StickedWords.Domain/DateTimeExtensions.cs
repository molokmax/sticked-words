namespace StickedWords.Domain;

public static class DateTimeExtensions
{
    public static long ToUnixTime(this DateTimeOffset date) =>
        date.ToUnixTimeMilliseconds();
}
