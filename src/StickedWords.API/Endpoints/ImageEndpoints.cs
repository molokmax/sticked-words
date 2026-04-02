using MediatR;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.Application.Commands.Images;
using StickedWords.Application.Queries.Images;

namespace StickedWords.API.Endpoints;

public static class ImageEndpoints
{
    public static IEndpointRouteBuilder RegisterImageEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/images");

        group.MapGet("/{id}", GetImage);
        group.MapPost("/", AddImage);
        group.MapDelete("/{id}", DeleteImage);

        return builder;
    }

    private static async Task<IResult> GetImage(
        long id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetImageByIdQuery(id);
        var result = await mediator.Send(request, cancellationToken);

        return Results.FileFromBase64(result);
    }

    private static async Task<long> AddImage(
        CreateImageRequestDto request,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var image = await mediator.Send(request.ToCommand(), cancellationToken);

        return image.Id;
    }

    private static async Task DeleteImage(
        long id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteImageCommand(id), cancellationToken);
    }
}
