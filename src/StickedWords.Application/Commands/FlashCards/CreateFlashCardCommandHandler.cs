using MediatR;
using StickedWords.Application.Services;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.FlashCards;

internal sealed class CreateFlashCardCommandHandler : IRequestHandler<CreateFlashCardCommand, FlashCard>
{
    private readonly IFlashCardRepository _repository;
    private readonly UserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public CreateFlashCardCommandHandler(
        IFlashCardRepository repository,
        UserService userService,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<FlashCard> Handle(CreateFlashCardCommand command, CancellationToken cancellationToken)
    {
        var user = await _userService.GetOrCreate(cancellationToken);
        var flashCard = FlashCard.Create(
            command.Word,
            command.Translation,
            command.ImageId,
            user,
            _timeProvider);
        _repository.Add(flashCard);
        await _unitOfWork.SaveChanges(cancellationToken);

        return flashCard;
    }
}
