using MediatR;
using StickedWords.Application.Services;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models.Exercises;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.Exercises;

internal sealed class GetTranslateToForeignQueryHandler : IRequestHandler<GetTranslateToForeignQuery, TranslateExercise>
{
    private readonly IFlashCardRepository _repository;
    private readonly UserService _userService;
    private readonly AccessPolicy _accessPolicy;

    public GetTranslateToForeignQueryHandler(
        IFlashCardRepository repository,
        UserService userService,
        AccessPolicy accessPolicy)
    {
        _repository = repository;
        _userService = userService;
        _accessPolicy = accessPolicy;
    }

    public async Task<TranslateExercise> Handle(GetTranslateToForeignQuery request, CancellationToken cancellationToken)
    {
        var user = await _userService.Get(cancellationToken);
        var flashCard = await _repository.GetById(request.FlashCardId, cancellationToken);
        if (flashCard is null)
        {
            throw new FlashCardNotFoundException(request.FlashCardId);
        }

        _accessPolicy.Check(user, flashCard);

        return new()
        {
            Word = flashCard.Translation,
            ImageId = flashCard.ImageId
        };
    }
}
