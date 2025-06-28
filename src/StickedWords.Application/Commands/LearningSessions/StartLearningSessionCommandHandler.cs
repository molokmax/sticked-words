using MediatR;
using Microsoft.Extensions.Options;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class StartLearningSessionCommandHandler : IRequestHandler<StartLearningSessionCommand, LearningSession>
{
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly LearningSessionOptions _options;

    public StartLearningSessionCommandHandler(
        ILearningSessionRepository sessionRepository,
        IFlashCardRepository flashCardRepository,
        IOptions<LearningSessionOptions> options)
    {
        _sessionRepository = sessionRepository;
        _flashCardRepository = flashCardRepository;
        _options = options.Value;
    }

    public async Task<LearningSession> Handle(StartLearningSessionCommand command, CancellationToken cancellationToken)
    {
        var activeSession = await _sessionRepository.GetActive(cancellationToken);
        if (activeSession is not null)
        {
            throw new LearningSessionAlreadyExistsException();
        }

        // TODO: more intellectual method to select words
        var flashCards = await _flashCardRepository.GetByQuery(new() { Take = _options.FlashCardCount }, cancellationToken);
        var learningSession = LearningSession.Create(flashCards.Data);
        learningSession.Start(_options.ExpireAfter);
        await _sessionRepository.Add(learningSession, cancellationToken);

        return learningSession;
    }
}
