using MediatR;
using Microsoft.Extensions.Options;
using StickedWords.Domain;
using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Models;
using StickedWords.Domain.Models.Exercises;
using StickedWords.Domain.Repositories;

namespace StickedWords.Application.Commands.Exercises;

internal sealed class CheckTranslateToForeignCommandHandler : IRequestHandler<CheckTranslateToForeignCommand, TranslateGuessResult>
{
    private readonly LearningSessionOptions _options;
    private readonly ILearningSessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckTranslateToForeignCommandHandler(
        IOptions<LearningSessionOptions> options,
        ILearningSessionRepository sessionRepository,
        IUnitOfWork unitOfWork)
    {
        _options = options.Value;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TranslateGuessResult> Handle(CheckTranslateToForeignCommand command, CancellationToken cancellationToken)
    {
        var activeSession = await _sessionRepository.GetActive(cancellationToken);
        if (activeSession is null)
        {
            throw new LearningSessionNotStartedException();
        }

        var activeStage = activeSession.GetActiveStage();
        if (activeStage.ExerciseType is not ExerciseType.TranslateNativeToForeign)
        {
            throw new WrongExerciseTypeException();
        }

        if (activeStage.CurrentFlashCard?.FlashCardId != command.FlashCardId)
        {
            throw new AnotherCurrentFlashCardException();
        }

        var flashCard = activeStage.CurrentFlashCard.FlashCard;
        var guessResult = GetGuessResult(flashCard, FlashCardWord.Create(command.Answer));

        if (!activeSession.TryMoveToNextFlashCard(guessResult))
        {
            activeSession.Finish(_options);
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        var result = guessResult is GuessResult.Correct
            ? TranslateGuessResult.Correct()
            : TranslateGuessResult.Wrong(flashCard.Translation);

        return result;
    }

    private static GuessResult GetGuessResult(FlashCard flashCard, FlashCardWord translation)
    {
        var isCorrect = FlashCardWord.Create(flashCard.Word) == translation;

        return isCorrect ? GuessResult.Correct : GuessResult.Wrong;
    }
}
