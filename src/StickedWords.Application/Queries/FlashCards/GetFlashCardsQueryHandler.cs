using MediatR;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.FlashCards;

internal sealed class GetFlashCardsQueryHandler : IRequestHandler<GetFlashCardsQuery, PageResult<FlashCard>>
{
    private readonly IFlashCardRepository _repository;

    public GetFlashCardsQueryHandler(IFlashCardRepository repository)
    {
        _repository = repository;
    }

    public async Task<PageResult<FlashCard>> Handle(GetFlashCardsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByQuery(request.PageQuery, cancellationToken);
    }
}
