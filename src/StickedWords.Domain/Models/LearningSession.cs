using StickedWords.Domain.Exceptions;

namespace StickedWords.Domain.Models;

public record LearningSession
{
    private LearningSession() { }

    public long Id { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public DateTimeOffset ExpiringAt { get; private set; }

    public LearningSessionState State { get; private set; } // TODO: how to expire session?

    public List<SessionStage> Stages { get; private set; } = [];

    public List<SessionFlashCard> FlashCards { get; private set; } = []; // TODO: how to make this collection readonly?

    public void Start(TimeSpan expireAfter)
    {
        State = LearningSessionState.Active;
        StartedAt = DateTimeOffset.UtcNow;
        ExpiringAt = DateTimeOffset.UtcNow.Add(expireAfter);

        var stage = Stages.OrderBy(x => x.OrdNumber).First();
        stage.Activate(FlashCards);
    }

    public void Finish(LearningSessionOptions options, TimeProvider timeProvider)
    {
        if (State is not LearningSessionState.Active)
        {
            throw new LearningSessionNotActiveException();
        }
        var activeStage = GetActiveStage();
        activeStage.Unactivate();
        State = LearningSessionState.Finished;

        foreach (var flashCard in FlashCards.Select(x => x.FlashCard))
        {
            var rate = GetFlashCardRate(flashCard);
            flashCard.UpdateBaseRate(rate, options, timeProvider);
        }
    }

    public SessionStage GetActiveStage() =>
        Stages.FirstOrDefault(x => x.IsActive) ?? throw new ActiveStageNotFoundException();

    public bool TryMoveToNextFlashCard(GuessResult guessResult)
    {
        var activeStage = GetActiveStage();
        activeStage.AddGuessResult(guessResult);
        if (activeStage.TryMoveToNextFlashCard(FlashCards))
        {
            return true;
        }
        if (TryMoveToNextSessionStage(activeStage))
        {
            return true;
        }

        return false;
    }

    private bool TryMoveToNextSessionStage(SessionStage sessionStage)
    {
        var stage = Stages.FirstOrDefault(x => x.OrdNumber == sessionStage.OrdNumber + 1);
        if (stage is null)
        {
            return false;
        }
        sessionStage.Unactivate();
        stage.Activate(FlashCards);

        return true;
    }

    private int GetFlashCardRate(FlashCard flashCard)
    {
        var correctGuessCount = 0d;
        foreach (var stage in Stages)
        {
            var guess = stage.Guesses.FirstOrDefault(x => x.FlashCardId == flashCard.Id);
            if (guess?.Result is GuessResult.Correct)
            {
                correctGuessCount += 1;
            }
        }
        var rate = Math.Floor(correctGuessCount / Stages.Count * 100);

        return Convert.ToInt32(rate);
    }

    public static LearningSession Create(IEnumerable<FlashCard> flashCards)
    {
        var session = new LearningSession();
        session.Stages = // TODO: types and ord numbers take from options. think about it
        [
            SessionStage.Create(0, ExerciseType.TranslateForeignToNative, session),
            SessionStage.Create(1, ExerciseType.TranslateNativeToForeign, session)
        ];
        session.FlashCards = flashCards
            .Select(x => new SessionFlashCard { FlashCard = x, LearningSession = session })
            .ToList();

        return session;
    }
}
