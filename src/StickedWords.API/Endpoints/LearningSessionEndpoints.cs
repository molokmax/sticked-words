using MediatR;
using StickedWords.API.Mappers;
using StickedWords.API.Models;
using StickedWords.Application.Commands.LearningSessions;

namespace StickedWords.API.Endpoints;

public static class LearningSessionEndpoints
{
    public static IEndpointRouteBuilder RegisterLearningSessionEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/api/learning-sessions");

        group.MapGet("/{id}", GetLearningSessionById);
        group.MapGet("/", GetActiveLearningSession);
        group.MapGet("/{id}/results", GetLearningSessionResults);
        group.MapPost("/", CreateLearningSession);

        return builder;
    }

    private static async Task<LearningSessionDto?> GetActiveLearningSession(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetActiveLearningSessionQuery();
        var session = await mediator.Send(request, cancellationToken);
        var result = session?.ToDto();

        return result;
    }

    private static async Task<LearningSessionDto?> GetLearningSessionById(
        long id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetLearningSessionByIdQuery(id);
        var session = await mediator.Send(request, cancellationToken);
        var result = session?.ToDto();

        return result;
    }

    private static async Task<LearningSessionResultsDto> GetLearningSessionResults(
        long id,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetLearningSessionResultsQuery(id);
        var result = await mediator.Send(request, cancellationToken);

        return result.ToDto();
    }

    private static async Task<LearningSessionDto> CreateLearningSession(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new StartLearningSessionCommand();
        var session = await mediator.Send(request, cancellationToken);
        var result = session.ToDto();

        return result;
    }
}
