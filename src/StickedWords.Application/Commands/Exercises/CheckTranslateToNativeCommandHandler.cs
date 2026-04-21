using MediatR;
using Microsoft.Extensions.Options;
using StickedWords.Application.Services;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Exercises;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.Exercises;

internal sealed class CheckTranslateToNativeCommandHandler : IRequestHandler<CheckTranslateToNativeCommand, GuessResult>
{
    private readonly LearningSessionOptions _options;
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly UserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeProvider _timeProvider;

    public CheckTranslateToNativeCommandHandler(
        IOptions<LearningSessionOptions> options,
        ILearningSessionRepository sessionRepository,
        UserService userService,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider)
    {
        _options = options.Value;
        _sessionRepository = sessionRepository;
        _userService = userService;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<GuessResult> Handle(CheckTranslateToNativeCommand command, CancellationToken cancellationToken)
    {
        var user = await _userService.Get(cancellationToken);
        var activeSession = await _sessionRepository.GetActive(user, cancellationToken);
        if (activeSession is null)
        {
            throw new LearningSessionNotStartedException();
        }

        var activeStage = activeSession.GetActiveStage();
        if (activeStage is null)
        {
            throw new ActiveStageNotFoundException();
        }

        if (activeStage.ExerciseType is not ExerciseType.TranslateForeignToNative)
        {
            throw new WrongExerciseTypeException();
        }

        if (activeStage.CurrentFlashCard?.FlashCardId != command.FlashCardId)
        {
            throw new AnotherCurrentFlashCardException();
        }

        var flashCard = activeStage.CurrentFlashCard.FlashCard;
        var guessResult = GetVerdict(flashCard, FlashCardWord.Create(command.Answer));

        if (!activeSession.TryMoveToNextFlashCard(guessResult))
        {
            activeSession.Finish(_options, _timeProvider);
        }

        activeSession.TryToExpire(_options, _timeProvider);

        await _unitOfWork.SaveChanges(cancellationToken);

        var result = guessResult is Verdict.Correct
            ? GuessResult.Correct(activeSession.State)
            : GuessResult.Wrong(flashCard.Translation, activeSession.State);

        return result;
    }

    private static Verdict GetVerdict(FlashCard flashCard, FlashCardWord translation)
    {
        var isCorrect = FlashCardWord.Create(flashCard.Translation) == translation;

        return isCorrect ? Verdict.Correct : Verdict.Wrong;
    }
}
