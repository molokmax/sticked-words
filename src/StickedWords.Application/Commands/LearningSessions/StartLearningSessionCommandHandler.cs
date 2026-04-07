using MediatR;
using Microsoft.Extensions.Options;
using StickedWords.Application.Services;
using StickedWords.Application.Specifications;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.LearningSessions;

internal sealed class StartLearningSessionCommandHandler : IRequestHandler<StartLearningSessionCommand, LearningSession>
{
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly IFlashCardRepository _flashCardRepository;
    private readonly UserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;
    private readonly LearningSessionOptions _options;

    public StartLearningSessionCommandHandler(
        ILearningSessionRepository sessionRepository,
        IFlashCardRepository flashCardRepository,
        UserService userService,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        IOptions<LearningSessionOptions> options)
    {
        _sessionRepository = sessionRepository;
        _flashCardRepository = flashCardRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
        _options = options.Value;
    }

    public async Task<LearningSession> Handle(StartLearningSessionCommand command, CancellationToken cancellationToken)
    {
        var now = _timeProvider.GetUtcNow();
        var user = await _userService.GetOrCreate(cancellationToken);
        var activeSession = await _sessionRepository.GetActiveNotExpired(user, now, cancellationToken);
        if (activeSession is not null)
        {
            throw new LearningSessionAlreadyExistsException();
        }

        // TODO: more intellectual method to select words
        var specification = new FlashCardsToRepeatSpecification(user, now, take: _options.FlashCardCount);
        var flashCards = await _flashCardRepository.GetBySpecification(specification, false, cancellationToken);
        // TODO: what if there is no words to learn?
        var learningSession = LearningSession.Create(flashCards.Data, user);
        learningSession.Start(_options.ExpireAfter);
        _sessionRepository.Add(learningSession);
        await _unitOfWork.SaveChanges(cancellationToken);

        return learningSession;
    }
}
