using StickedWords.Domain;
using StickedWords.Domain.Models;

namespace StickedWords.UnitTests;

public class LearningSessionTests
{
    [Test]
    public void Finish_StartedSession_StateFinished()
    {
        // Arrange
        var session = GetStartedSession([GetFlashCard()]);
        var options = GetOptions();

        // Act
        session.Finish(options);

        // Assert
        Assert.That(session.State, Is.EqualTo(LearningSessionState.Finished));
    }

    [Test]
    public void Finish_StartedSession_StagesUnactive()
    {
        // Arrange
        var session = GetStartedSession([GetFlashCard()]);
        var options = GetOptions();

        // Act
        session.Finish(options);

        // Assert
        foreach (var stage in session.Stages)
        {
            Assert.That(stage.IsActive, Is.False);
        }
    }

    [Test]
    public void Finish_AllGuessesCorrect_RateIs100()
    {
        // Arrange
        var flashCard = GetFlashCard();
        var session = GetStartedSession([flashCard]);
        var options = GetOptions();
        session.TryMoveToNextFlashCard(GuessResult.Correct);
        session.TryMoveToNextFlashCard(GuessResult.Correct);

        // Act
        session.Finish(options);

        // Assert
        Assert.That(flashCard.Rate, Is.EqualTo(100));
    }

    [Test]
    public void Finish_HalfGuessesCorrect_RateIs50()
    {
        // Arrange
        var flashCard = GetFlashCard();
        var session = GetStartedSession([flashCard]);
        var options = GetOptions();
        session.TryMoveToNextFlashCard(GuessResult.Correct);
        session.TryMoveToNextFlashCard(GuessResult.Wrong);

        // Act
        session.Finish(options);

        // Assert
        Assert.That(flashCard.Rate, Is.EqualTo(50));
    }

    [Test]
    public void Finish_AllGuessesWrong_RateIs0()
    {
        // Arrange
        var flashCard = GetFlashCard();
        var session = GetStartedSession([flashCard]);
        var options = GetOptions();
        session.TryMoveToNextFlashCard(GuessResult.Wrong);
        session.TryMoveToNextFlashCard(GuessResult.Wrong);

        // Act
        session.Finish(options);

        // Assert
        Assert.That(flashCard.Rate, Is.EqualTo(0));
    }

    [Test]
    public void Finish_NoGuesses_RateIs0()
    {
        // Arrange
        var flashCard = GetFlashCard();
        var session = GetStartedSession([flashCard]);
        var options = GetOptions();

        // Act
        session.Finish(options);

        // Assert
        Assert.That(flashCard.Rate, Is.EqualTo(0));
    }

    private static LearningSession GetStartedSession(FlashCard[] flashCards)
    {
        var session = LearningSession.Create(flashCards);
        session.Start(TimeSpan.FromMinutes(1));

        return session;
    }

    private static FlashCard GetFlashCard() => FlashCard.Create("word1", "слово1");

    private static LearningSessionOptions GetOptions()
    {
        return new()
        {
            RepeatFlashCardPeriod = TimeSpan.FromMinutes(5)
        };
    }
}
