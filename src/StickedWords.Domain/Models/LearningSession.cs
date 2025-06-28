namespace StickedWords.Domain.Models;

public record LearningSession
{
    private LearningSession() { }

    public long Id { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public DateTimeOffset ExpiringAt { get; private set; }

    public LearningSessionState State { get; private set; } // TODO: как экспайрить?

    public List<SessionStage> Stages { get; private set; } = [];

    public List<SessionFlashCard> FlashCards { get; private set; } = []; // TODO: как сделать коллекцию неизменяемой?

    public void Start(TimeSpan expireAfter)
    {
        State = LearningSessionState.Active;
        StartedAt = DateTimeOffset.UtcNow;
        ExpiringAt = DateTimeOffset.UtcNow.Add(expireAfter);

        var stage = Stages.OrderBy(x => x.OrdNumber).First();
        stage.Activate(FlashCards);
    }

    public static LearningSession Create(IEnumerable<FlashCard> flashCards)
    {
        var session = new LearningSession();
        session.Stages = // TODO: types and ord numbers take from options. think about it
        [
            new()
            {
                OrdNumber = 0,
                ExerciseType = ExerciseType.TranslateForeignToNative,
                LearningSession = session
            },
            new()
            {
                OrdNumber = 1,
                ExerciseType = ExerciseType.TranslateNativeToForeign,
                LearningSession = session
            }
        ];
        session.FlashCards = flashCards
            .Select(x => new SessionFlashCard { FlashCard = x, LearningSession = session })
            .ToList();

        return session;
    }
}
