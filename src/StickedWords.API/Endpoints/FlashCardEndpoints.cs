using MediatR;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.Application.Queries.FlashCards;

namespace StickedWords.API.Endpoints;

public static class FlashCardEndpoints
{
    public static IEndpointRouteBuilder RegisterFlashCardEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/flash-cards");

        group.MapGet("/", GetFlashCardList);

        return builder;
    }

    private static async Task<PageResultDto<FlashCardShortDto>> GetFlashCardList(
        PageQueryDto query,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetFlashCardsQuery(query.ToDomain());
        var page = await mediator.Send(request, cancellationToken);

        var result = page.ToDto(FlashCardMapper.ToShortDto);

        return result;
    }
}
