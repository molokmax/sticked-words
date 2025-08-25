using StickedWords.Domain.Exceptions;
using StickedWords.Domain.Repositories;

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

    public SessionStage? GetActiveStage() => Stages.FirstOrDefault(x => x.IsActive);

    public bool TryMoveToNextFlashCard(GuessResult guessResult)
    {
        var activeStage = GetActiveStage();
        if (activeStage is null)
        {
            throw new ActiveStageNotFoundException();
        }
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
