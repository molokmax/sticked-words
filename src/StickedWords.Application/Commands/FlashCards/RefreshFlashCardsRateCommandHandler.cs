using MediatR;
using Microsoft.Extensions.Options;
using StickedWords.Application.Specifications;
using StickedWords.Domain;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.FlashCards;

internal sealed class RefreshFlashCardsRateCommandHandler : IRequestHandler<RefreshFlashCardsRateCommand>
{
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    private readonly LearningSessionOptions _options;

    public RefreshFlashCardsRateCommandHandler(
        IFlashCardRepository flashCardRepository,
        ILearningSessionRepository sessionRepository,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        IOptions<LearningSessionOptions> options)
    {
        _flashCardRepository = flashCardRepository;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
        _options = options.Value;
    }

    public async Task Handle(RefreshFlashCardsRateCommand command, CancellationToken cancellationToken)
    {
        var activeSession = await _sessionRepository.GetActive(cancellationToken);
        if (activeSession is not null)
        {
            return;
        }

        var now = _timeProvider.GetUtcNow();
        var includeTotal = true;
        var processed = 0;
        int? total = null;
        while (total is null || processed < total)
        {
            var specification = new FlashCardsToRepeatSpecification(now, processed);
            var page = await _flashCardRepository
                .GetBySpecification(specification, includeTotal, cancellationToken);
            if (total is null)
            {
                total = page.Total;
                includeTotal = false;
            }
            foreach (var flashCard in page.Data)
            {
                flashCard.RefreshRate(_options, _timeProvider);
            }
            processed += page.Data.Count;
        }


        await _unitOfWork.SaveChanges(cancellationToken);
    }
}
