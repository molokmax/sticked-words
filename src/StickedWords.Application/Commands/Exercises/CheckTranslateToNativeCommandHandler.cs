using MediatR;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Exercises;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.Exercises;

internal sealed class CheckTranslateToNativeCommandHandler : IRequestHandler<CheckTranslateToNativeCommand, TranslateGuessResult>
{
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckTranslateToNativeCommandHandler(
        ILearningSessionRepository sessionRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TranslateGuessResult> Handle(CheckTranslateToNativeCommand command, CancellationToken cancellationToken)
    {
        var activeSession = await _sessionRepository.GetActive(cancellationToken);
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
        var guessResult = GetGuessResult(flashCard, FlashCardWord.Create(command.Answer));

        activeSession.TryMoveToNextFlashCard(guessResult);
        // TODO: recalculate Rate of flash card

        await _unitOfWork.SaveChanges(cancellationToken);

        var result = guessResult is GuessResult.Correct
            ? TranslateGuessResult.Correct()
            : TranslateGuessResult.Wrong(flashCard.Translation);

        return result;
    }

    private static GuessResult GetGuessResult(FlashCard flashCard, FlashCardWord translation)
    {
        var isCorrect = FlashCardWord.Create(flashCard.Translation) == translation;

        return isCorrect ? GuessResult.Correct : GuessResult.Wrong;
    }
}
