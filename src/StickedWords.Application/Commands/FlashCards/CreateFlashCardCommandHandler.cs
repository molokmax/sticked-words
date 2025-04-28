using MediatR;
using StickedWords.Application.Commands.FlashCards;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.FlashCards;

internal sealed class CreateFlashCardCommandHandler : IRequestHandler<CreateFlashCardCommand, FlashCard>
{
    private readonly IFlashCardRepository _repository;

    public CreateFlashCardCommandHandler(IFlashCardRepository repository)
    {
        _repository = repository;
    }

    public async Task<FlashCard> Handle(CreateFlashCardCommand command, CancellationToken cancellationToken)
    {
        var flashCard = new FlashCard
        {
            Word = command.Word,
            Translation = command.Translation,
            CreatedAt = DateTimeOffset.UtcNow
        };
        await _repository.Add(flashCard, cancellationToken);

        return flashCard;
    }
}
