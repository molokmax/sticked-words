using MediatR;
using StickedWords.Application.Commands.LearningSessions;
using TickerQ.Utilities.Base;

namespace StickedWords.Background;

public sealed class ExpireLearningSessionsJob
{
    private readonly IMediator _mediator;

    public ExpireLearningSessionsJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    [TickerFunction(nameof(ExpireLearningSessionsJob), cronExpression: "0 30 23 * * *")]
    public async Task Run(TickerFunctionContext context, CancellationToken cancellationToken)
    {
        context.CronOccurrenceOperations.SkipIfAlreadyRunning();

        await _mediator.Send(new ExpireLearningSessionsCommand(), cancellationToken);
    }
}
