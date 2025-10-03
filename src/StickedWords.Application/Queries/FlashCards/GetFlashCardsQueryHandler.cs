using MediatR;
using StickedWords.Application.Specifications;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.FlashCards;

internal sealed class GetFlashCardsQueryHandler : IRequestHandler<GetFlashCardsQuery, PageResult<FlashCard>>
{
    private readonly IFlashCardRepository _repository;
    private readonly TimeProvider _timeProvider;

    public GetFlashCardsQueryHandler(
        IFlashCardRepository repository,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _timeProvider = timeProvider;
    }

    public async Task<PageResult<FlashCard>> Handle(
        GetFlashCardsQuery request,
        CancellationToken cancellationToken)
    {
        var specification = new FlashCardsToRepeatSpecification(_timeProvider.GetUtcNow());
        return await _repository.GetBySpecification(
            specification,
            request.PageQuery.IncludeTotal,
            cancellationToken);
    }
}
