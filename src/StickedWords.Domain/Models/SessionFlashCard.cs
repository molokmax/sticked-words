namespace StickedWords.Domain.Models;

public record SessionFlashCard
{
    public long Id { get; init; }

    public long LearningSessionId { get; init; }
    
    public required virtual LearningSession LearningSession { get; init; }

    public long FlashCardId { get; init; }

    public required virtual FlashCard FlashCard { get; init; }
}
