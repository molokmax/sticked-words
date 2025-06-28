namespace StickedWords.Domain.Models;

public record SessionStage // TODO: сделать метод Create
{
    public long Id { get; init; }

    public int OrdNumber {  get; init; }

    public bool IsActive { get; private set; }

    public ExerciseType ExerciseType { get; init; }

    public long LearningSessionId { get; init; }

    public required virtual LearningSession LearningSession { get; init; }

    public long? CurrentFlashCardId { get; private set; }

    public virtual SessionFlashCard? CurrentFlashCard { get; private set; }

    public void Activate(IReadOnlyCollection<SessionFlashCard> flashCards)
    {
        IsActive = true;
        var flashCardIndex = Random.Shared.Next(flashCards.Count);
        CurrentFlashCard = flashCards.ElementAt(flashCardIndex);
    }
}
