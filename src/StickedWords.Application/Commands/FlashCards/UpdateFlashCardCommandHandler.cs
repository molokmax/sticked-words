using MediatR;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.FlashCards;

internal sealed class UpdateFlashCardCommandHandler : IRequestHandler<UpdateFlashCardCommand, FlashCard>
{
    private readonly IFlashCardRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFlashCardCommandHandler(
        IFlashCardRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<FlashCard> Handle(UpdateFlashCardCommand command, CancellationToken cancellationToken)
    {
        var flashCard = await _repository.GetById(command.FlashCardId, cancellationToken)
            ?? throw new FlashCardNotFoundException(command.FlashCardId);
        flashCard.UpdateWord(FlashCardWord.Create(command.Word), FlashCardWord.Create(command.Translation));
        await _unitOfWork.SaveChanges(cancellationToken);

        return flashCard;
    }
}
