using MediatR;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.API.Models.Exercises;
using StickedWords.Application.Commands.Exercises;
using StickedWords.Application.Queries.Exercises;

namespace StickedWords.API.Endpoints;

public static class ChooseFromNativeExerciseEndpoints
{
    public static IEndpointRouteBuilder RegisterChooseFromNativeExerciseEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/exercises/choose-from-native").RequireAuthorization();

        group.MapGet("/", GetExercise);
        group.MapPost("/", CheckGuess);

        return builder;
    }

    private static async Task<ChooseExerciseDto> GetExercise(
        long flashCardId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetChooseFromNativeQuery(flashCardId);
        var exercise = await mediator.Send(request, cancellationToken);

        return exercise.ToDto();
    }

    private static async Task<GuessResultDto> CheckGuess(
        GuessDto guess,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CheckChooseFromNativeCommand
        {
            FlashCardId = guess.FlashCardId,
            Answer = guess.Answer
        };
        var result = await mediator.Send(command, cancellationToken);

        return result.ToDto();
    }
}
