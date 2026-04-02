namespace StickedWords.Domain.Exceptions;

public sealed class ImageNotFoundException : Exception
{
    private static readonly string _message = "Image not found by id";

    public ImageNotFoundException(long imageId) : base(_message)
    {
        ImageId = imageId;
    }

    public long ImageId { get; }
}
