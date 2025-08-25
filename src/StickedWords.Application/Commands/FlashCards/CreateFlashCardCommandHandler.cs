using MediatR;
using StickedWords.Domain;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.FlashCards;

internal sealed class CreateFlashCardCommandHandler : IRequestHandler<CreateFlashCardCommand, FlashCard>
{
    private readonly IFlashCardRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFlashCardCommandHandler(
        IFlashCardRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FlashCard> Handle(CreateFlashCardCommand command, CancellationToken cancellationToken)
    {
        var flashCard = FlashCard.Create(command.Word, command.Translation);
        _repository.Add(flashCard);
        await _unitOfWork.SaveChanges(cancellationToken);

        return flashCard;
    }
}
