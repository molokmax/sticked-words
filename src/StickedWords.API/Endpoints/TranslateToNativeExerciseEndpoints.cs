using MediatR;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.API.Models.Exercises;
using StickedWords.Application.Commands.Exercises;
using StickedWords.Application.Queries.Exercises;

namespace StickedWords.API.Endpoints;

public static class TranslateToNativeExerciseEndpoints
{
    public static IEndpointRouteBuilder RegisterTranslateToNativeExerciseEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/exercises/translate-to-native");

        group.MapGet("/", GetExercise);
        group.MapPost("/", CheckGuess);

        return builder;
    }

    private static async Task<TranslateExerciseDto> GetExercise(
        long flashCardId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetTranslateToNativeQuery(flashCardId);
        var exercise = await mediator.Send(request, cancellationToken);
        var result = exercise.ToDto();

        return result;
    }

    private static async Task<TranslateGuessResultDto> CheckGuess(
        TranslateGuessDto guess,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CheckTranslateToNativeCommand
        {
            FlashCardId = guess.FlashCardId,
            Answer = guess.Answer
        };
        var result = await mediator.Send(command, cancellationToken);

        return result.ToDto();
    }
}
