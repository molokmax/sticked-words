using MediatR;
using StickedWords.Application.Commands.FlashCards;
using TickerQ.Utilities.Base;

namespace StickedWords.Background;

public sealed class RefreshFlashCardRateJob
{
    private readonly IMediator _mediator;

    public RefreshFlashCardRateJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    [TickerFunction(nameof(RefreshFlashCardRateJob), cronExpression: "10 * * * *")]
    public async Task Run(CancellationToken cancellationToken)
    {
        await _mediator.Send(new RefreshFlashCardsRateCommand(), cancellationToken);
    }
}
