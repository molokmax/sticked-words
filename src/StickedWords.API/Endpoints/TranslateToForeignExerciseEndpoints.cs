using MediatR;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.API.Models.Exercises;
using StickedWords.Application.Commands.Exercises;
using StickedWords.Application.Queries.Exercises;

namespace StickedWords.API.Endpoints;

public static class TranslateToForeignExerciseEndpoints
{
    public static IEndpointRouteBuilder RegisterTranslateToForeignExerciseEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/exercises/translate-to-foreign");

        group.MapGet("/", GetExercise);
        group.MapPost("/", CheckGuess);

        return builder;
    }

    private static async Task<TranslateExerciseDto> GetExercise(
        long flashCardId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetTranslateToForeignQuery(flashCardId);
        var exercise = await mediator.Send(request, cancellationToken);

        return exercise.ToDto();
    }

    private static async Task<TranslateGuessResultDto> CheckGuess(
        TranslateGuessDto guess,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CheckTranslateToForeignCommand
        {
            FlashCardId = guess.FlashCardId,
            Answer = guess.Answer
        };
        var result = await mediator.Send(command, cancellationToken);

        return result.ToDto();
    }
}
