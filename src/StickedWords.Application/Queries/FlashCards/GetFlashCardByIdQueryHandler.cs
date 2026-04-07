using MediatR;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.FlashCards;

internal sealed class GetFlashCardByIdQueryHandler : IRequestHandler<GetFlashCardByIdQuery, FlashCard>
{
    private readonly IFlashCardRepository _repository;

    public GetFlashCardByIdQueryHandler(
        IFlashCardRepository repository)
    {
        _repository = repository;
    }

    public async Task<FlashCard> Handle(
        GetFlashCardByIdQuery request,
        CancellationToken cancellationToken)
    {
        // TODO: check that user has permissions
        return await _repository.GetById(request.FlashCardId, cancellationToken)
            ?? throw new FlashCardNotFoundException(request.FlashCardId);
    }
}
