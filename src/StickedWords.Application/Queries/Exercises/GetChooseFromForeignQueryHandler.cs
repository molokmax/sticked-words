using MediatR;
using StickedWords.Application.Services;
using StickedWords.Application.Specifications;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models.Exercises;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.Exercises;

internal sealed class GetChooseFromForeignQueryHandler : IRequestHandler<GetChooseFromForeignQuery, ChooseExercise>
{
    private readonly IFlashCardRepository _repository;
    private readonly UserService _userService;
    private readonly AccessPolicy _accessPolicy;

    public GetChooseFromForeignQueryHandler(
        IFlashCardRepository repository,
        UserService userService,
        AccessPolicy accessPolicy)
    {
        _repository = repository;
        _userService = userService;
        _accessPolicy = accessPolicy;
    }

    public async Task<ChooseExercise> Handle(GetChooseFromForeignQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.Get(cancellationToken);
        var flashCard = await _repository.GetById(request.FlashCardId, cancellationToken);
        if (flashCard is null)
        {
            throw new FlashCardNotFoundException(request.FlashCardId);
        }

        _accessPolicy.Check(user, flashCard);

        var specification = new FlashCardsToChooseSpecification(user, flashCard);
        var wrongFlashCards = await _repository.GetBySpecification(specification, false, cancellationToken);
        string[] options = [flashCard.Word, .. wrongFlashCards.Data.Select(x => x.Word)];
        Random.Shared.Shuffle(options);

        return new()
        {
            Word = flashCard.Translation,
            ImageId = flashCard.ImageId,
            Options = options
        };
    }
}
