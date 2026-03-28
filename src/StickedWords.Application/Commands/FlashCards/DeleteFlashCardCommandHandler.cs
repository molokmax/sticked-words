using MediatR;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.FlashCards;

internal sealed class DeleteFlashCardCommandHandler : IRequestHandler<DeleteFlashCardCommand, FlashCard>
{
    private readonly IFlashCardRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public DeleteFlashCardCommandHandler(
        IFlashCardRepository repository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<FlashCard> Handle(DeleteFlashCardCommand command, CancellationToken cancellationToken)
    {
        var flashCard = await _repository.GetById(command.FlashCardId, cancellationToken)
            ?? throw new FlashCardNotFoundException(command.FlashCardId);
        flashCard.Delete(_timeProvider);
        await _unitOfWork.SaveChanges(cancellationToken);

        return flashCard;
    }
}
