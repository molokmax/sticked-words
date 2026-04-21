using MediatR;
using StickedWords.Application.Services;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.FlashCards;

internal sealed class UpdateFlashCardCommandHandler : IRequestHandler<UpdateFlashCardCommand, FlashCard>
{
    private readonly IFlashCardRepository _repository;
    private readonly UserService _userService;
    private readonly AccessPolicy _accessPolicy;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFlashCardCommandHandler(
        IFlashCardRepository repository,
        UserService userService,
        AccessPolicy accessPolicy,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userService = userService;
        _accessPolicy = accessPolicy;
        _unitOfWork = unitOfWork;
    }

    public async Task<FlashCard> Handle(UpdateFlashCardCommand command, CancellationToken cancellationToken)
    {
        var user = await _userService.Get(cancellationToken);
        var flashCard = await _repository.GetById(command.FlashCardId, cancellationToken)
            ?? throw new FlashCardNotFoundException(command.FlashCardId);

        _accessPolicy.Check(user, flashCard);

        flashCard.UpdateWord(command.Word, command.Translation, command.ImageId);
        _repository.Update(flashCard);
        await _unitOfWork.SaveChanges(cancellationToken);

        return flashCard;
    }
}
