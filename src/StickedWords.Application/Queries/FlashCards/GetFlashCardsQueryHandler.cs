using MediatR;
using StickedWords.Application.Services;
using StickedWords.Application.Specifications;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Queries.FlashCards;

internal sealed class GetFlashCardsQueryHandler : IRequestHandler<GetFlashCardsQuery, PageResult<FlashCard>>
{
    private readonly IFlashCardRepository _repository;
    private readonly UserService _userService;
    private readonly TimeProvider _timeProvider;

    public GetFlashCardsQueryHandler(
        IFlashCardRepository repository,
        UserService userService,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _userService = userService;
        _timeProvider = timeProvider;
    }

    public async Task<PageResult<FlashCard>> Handle(
        GetFlashCardsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetOrDefault(cancellationToken);
        if (user is null)
        {
            return new();
        }

        var specification = new FlashCardsToRepeatSpecification(
            user,
            _timeProvider.GetUtcNow(),
            request.PageQuery.Skip,
            request.PageQuery.Take);

        return await _repository.GetBySpecification(
            specification,
            request.PageQuery.IncludeTotal,
            cancellationToken);
    }
}
