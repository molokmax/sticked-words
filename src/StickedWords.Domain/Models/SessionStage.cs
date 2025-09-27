using StickedWords.Domain.Exceptions;

namespace StickedWords.Domain.Models;

public record SessionStage
{
    private SessionStage() { }

    public long Id { get; init; }

    public int OrdNumber { get; init; }

    public bool IsActive { get; private set; }

    public ExerciseType ExerciseType { get; init; }

    public long LearningSessionId { get; init; }

    public required virtual LearningSession LearningSession { get; init; }

    public long? CurrentFlashCardId { get; private set; }

    public virtual SessionFlashCard? CurrentFlashCard { get; private set; }

    public List<Guess> Guesses { get; private set; } = [];

    public void AddGuessResult(GuessResult guessResult)
    {
        if (CurrentFlashCard is null)
        {
            throw new CurrentFlashCardNotDefinedException(Id);
        }
        var guess = new Guess
        {
            FlashCardId = CurrentFlashCard.FlashCardId,
            FlashCard = CurrentFlashCard.FlashCard,
            SessionStage = this,
            Result = guessResult
        };
        Guesses.Add(guess);
    }

    public void Unactivate()
    {
        IsActive = false;
        CurrentFlashCard = null;
    }

    public void Activate(IReadOnlyList<SessionFlashCard> flashCards)
    {
        IsActive = true;
        var flashCardIndex = Random.Shared.Next(flashCards.Count);
        CurrentFlashCard = flashCards[flashCardIndex];
    }

    public bool TryMoveToNextFlashCard(IReadOnlyCollection<SessionFlashCard> flashCards)
    {
        var guessedFlashCardIds = Guesses.Select(x => x.FlashCardId);
        var restFlashCards = flashCards.ExceptBy(guessedFlashCardIds, x => x.FlashCardId).ToList();
        if (restFlashCards.Count == 0)
        {
            return false;
        }

        var flashCardIndex = Random.Shared.Next(restFlashCards.Count);
        CurrentFlashCard = restFlashCards[flashCardIndex];

        return true;
    }

    public static SessionStage Create(int ordNumber, ExerciseType exerciseType, LearningSession session)
    {
        return new SessionStage
        {
            OrdNumber = ordNumber,
            ExerciseType = exerciseType,
            LearningSession = session
        };
    }
}
