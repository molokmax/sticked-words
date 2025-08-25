using MediatR;
using StickedWords.Domain.Models.Exercises;

namespace StickedWords.Application.Commands.Exercises;

public record CheckTranslateToNativeCommand : IRequest<TranslateGuessResult>
{
    public long FlashCardId { get; init; }

    public required string Answer { get; init; }
}
