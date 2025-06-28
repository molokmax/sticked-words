using MediatR;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.Application.Queries.Exercises;

namespace StickedWords.API.Endpoints;

public static class TranslateToForeignExerciseEndpoints
{
    public static IEndpointRouteBuilder RegisterTranslateToForeignExerciseEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/exercises/translate-to-foreign");

        group.MapGet("/", GetExercise);

        return builder;
    }

    private static async Task<TranslateExerciseDto> GetExercise(
        long flashCardId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetTranslateToForeignQuery(flashCardId);
        var exercise = await mediator.Send(request, cancellationToken);
        var result = exercise.ToDto();

        return result;
    }
}
