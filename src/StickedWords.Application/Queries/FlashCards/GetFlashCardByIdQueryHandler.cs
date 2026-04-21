using MediatR;
using StickedWords.Application.Services;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.FlashCards;

internal sealed class GetFlashCardByIdQueryHandler : IRequestHandler<GetFlashCardByIdQuery, FlashCard>
{
    private readonly IFlashCardRepository _repository;
    private readonly UserService _userService;
    private readonly AccessPolicy _accessPolicy;

    public GetFlashCardByIdQueryHandler(
        IFlashCardRepository repository,
        UserService userService,
        AccessPolicy accessPolicy)
    {
        _repository = repository;
        _userService = userService;
        _accessPolicy = accessPolicy;
    }

    public async Task<FlashCard> Handle(
        GetFlashCardByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.Get(cancellationToken);
        var flashCard = await _repository.GetById(request.FlashCardId, cancellationToken)
            ?? throw new FlashCardNotFoundException(request.FlashCardId);

        _accessPolicy.Check(user, flashCard);

        return flashCard;
    }
}
