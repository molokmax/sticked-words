using MediatR;
using Microsoft.Extensions.Options;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Paging;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class StartLearningSessionCommandHandler : IRequestHandler<StartLearningSessionCommand, LearningSession>
{
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LearningSessionOptions _options;

    public StartLearningSessionCommandHandler(
        ILearningSessionRepository sessionRepository,
        IFlashCardRepository flashCardRepository,
        IUnitOfWork unitOfWork,
        IOptions<LearningSessionOptions> options)
    {
        _sessionRepository = sessionRepository;
        _flashCardRepository = flashCardRepository;
        _unitOfWork = unitOfWork;
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
        var pageQuery = new PageQuery { Take = _options.FlashCardCount };
        var flashCards = await _flashCardRepository.GetByQuery(pageQuery, cancellationToken);
        var learningSession = LearningSession.Create(flashCards.Data);
        learningSession.Start(_options.ExpireAfter);
        _sessionRepository.Add(learningSession);
        await _unitOfWork.SaveChanges(cancellationToken);

        return learningSession;
    }
}
