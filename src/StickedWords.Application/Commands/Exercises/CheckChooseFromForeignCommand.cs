using MediatR;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.Application.Commands.Exercises;

public record CheckChooseFromForeignCommand : IRequest<GuessResult>
{
    public long FlashCardId { get; init; }

    public required string Answer { get; init; }
}
