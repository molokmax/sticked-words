using MediatR;
using StickedWords.Domain.Models;

namespace StickedWords.Application.Commands.FlashCards;

public record CreateFlashCardCommand : IRequest<FlashCard>
{
    public required string Word { get; init; }

    public required string Translation { get; init; }
}
