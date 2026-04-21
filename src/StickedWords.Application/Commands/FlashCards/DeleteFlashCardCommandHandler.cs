using MediatR;
using StickedWords.Application.Services;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.FlashCards;

internal sealed class DeleteFlashCardCommandHandler : IRequestHandler<DeleteFlashCardCommand, FlashCard>
{
    private readonly IFlashCardRepository _repository;
    private readonly UserService _userService;
    private readonly AccessPolicy _accessPolicy;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public DeleteFlashCardCommandHandler(
        IFlashCardRepository repository,
        UserService userService,
        AccessPolicy accessPolicy,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _userService = userService;
        _accessPolicy = accessPolicy;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<FlashCard> Handle(DeleteFlashCardCommand command, CancellationToken cancellationToken)
    {
        var user = await _userService.Get(cancellationToken);
        var flashCard = await _repository.GetById(command.FlashCardId, cancellationToken)
            ?? throw new FlashCardNotFoundException(command.FlashCardId);

        _accessPolicy.Check(user, flashCard);

        flashCard.Delete(_timeProvider);
        await _unitOfWork.SaveChanges(cancellationToken);

        return flashCard;
    }
}
