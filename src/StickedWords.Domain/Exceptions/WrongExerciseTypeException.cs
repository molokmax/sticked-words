namespace StickedWords.Domain.Exceptions;

public sealed class WrongExerciseTypeException : Exception
{
    private static readonly string _message = "Wrong exercise type";

    public WrongExerciseTypeException() : base(_message) { }
}
