namespace StickedWords.Domain.Exceptions;

public sealed class RateIsNotValidException : Exception
{
    private static readonly string _message = "Rate must be between 0 and 100";

    public RateIsNotValidException(int rate) : base(_message)
    {
        Rate = rate;
    }


    public int Rate { get; }
}
