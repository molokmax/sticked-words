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

        group.MapGet("/", GetLearningSession);
        group.MapPost("/", AddLearningSession);

        return builder;
    }

    private static async Task<LearningSessionDto?> GetLearningSession(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetLearningSessionQuery();
        var session = await mediator.Send(request, cancellationToken);
        var result = session?.ToDto();

        return result;
    }

    private static async Task<LearningSessionDto> AddLearningSession(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new StartLearningSessionCommand();
        var session = await mediator.Send(request, cancellationToken);
        var result = session.ToDto();

        return result;
    }
}
