using MediatR;
using StickedWords.Domain.Models;

namespace StickedWords.Application.Commands.FlashCards;

public record UpdateFlashCardCommand : IRequest<FlashCard>
{
    public required long FlashCardId { get; init; }

    public required string Word { get; init; }

    public required string Translation { get; init; }

    public required long? ImageId { get; init; }
}
