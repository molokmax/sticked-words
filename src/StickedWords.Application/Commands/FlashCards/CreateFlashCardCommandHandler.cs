using MediatR;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.FlashCards;

internal sealed class CreateFlashCardCommandHandler : IRequestHandler<CreateFlashCardCommand, FlashCard>
{
    private readonly IFlashCardRepository _repository;

    public CreateFlashCardCommandHandler(IFlashCardRepository repository)
    {
        _repository = repository;
    }

    public async Task<FlashCard> Handle(CreateFlashCardCommand command, CancellationToken cancellationToken)
    {
        var flashCard = FlashCard.Create(command.Word, command.Translation);
        await _repository.Add(flashCard, cancellationToken);

        return flashCard;
    }
}
