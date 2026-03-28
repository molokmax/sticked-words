using MediatR;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.Application.Commands.FlashCards;
using StickedWords.Application.Queries.FlashCards;

namespace StickedWords.API.Endpoints;

public static class FlashCardEndpoints
{
    public static IEndpointRouteBuilder RegisterFlashCardEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/flash-cards");

        group.MapGet("/", GetFlashCardList);
        group.MapPost("/", AddFlashCard);
        group.MapDelete("/{id}", DeleteFlashCard);

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

    private static async Task<FlashCardShortDto> AddFlashCard(
        CreateFlashCardRequestDto request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var card = await mediator.Send(request.ToCommand(), cancellationToken);
        var result = card.ToShortDto();

        return result;
    }

    private static async Task<FlashCardShortDto> DeleteFlashCard(
        long id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var card = await mediator.Send(new DeleteFlashCardCommand(id), cancellationToken);
        var result = card.ToShortDto();

        return result;
    }
}
