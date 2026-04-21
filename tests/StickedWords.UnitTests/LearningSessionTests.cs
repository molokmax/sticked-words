using Microsoft.Extensions.Time.Testing;
using StickedWords.Domain;
using StickedWords.Domain.Models;

namespace StickedWords.UnitTests;

public class LearningSessionTests
{
    private static TimeProvider _timeProvider = new FakeTimeProvider(DateTimeOffset.UtcNow);
    private static User _user = User.Create("123");

    [Test]
    public void Finish_StartedSession_StateFinished()
    {
        // Arrange
        var options = GetOptions();
        var session = GetStartedSession([GetFlashCard()], options);

        // Act
        session.Finish(options, _timeProvider);

        // Assert
        Assert.That(session.State, Is.EqualTo(LearningSessionState.Finished));
    }

    [Test]
    public void Finish_StartedSession_StagesUnactive()
    {
        // Arrange
        var options = GetOptions();
        var session = GetStartedSession([GetFlashCard()], options);

        // Act
        session.Finish(options, _timeProvider);

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
        var options = GetOptions();
        var session = GetStartedSession([flashCard], options);
        session.TryMoveToNextFlashCard(Verdict.Correct);
        session.TryMoveToNextFlashCard(Verdict.Correct);

        // Act
        session.Finish(options, _timeProvider);

        // Assert
        Assert.That(flashCard.Rate, Is.EqualTo(100));
    }

    [Test]
    public void Finish_HalfGuessesCorrect_RateIs50()
    {
        // Arrange
        var flashCard = GetFlashCard();
        var options = GetOptions();
        var session = GetStartedSession([flashCard], options);
        session.TryMoveToNextFlashCard(Verdict.Correct);
        session.TryMoveToNextFlashCard(Verdict.Wrong);

        // Act
        session.Finish(options, _timeProvider);

        // Assert
        Assert.That(flashCard.Rate, Is.EqualTo(50));
    }

    [Test]
    public void Finish_AllGuessesWrong_RateIs0()
    {
        // Arrange
        var flashCard = GetFlashCard();
        var options = GetOptions();
        var session = GetStartedSession([flashCard], options);
        session.TryMoveToNextFlashCard(Verdict.Wrong);
        session.TryMoveToNextFlashCard(Verdict.Wrong);

        // Act
        session.Finish(options, _timeProvider);

        // Assert
        Assert.That(flashCard.Rate, Is.EqualTo(0));
    }

    [Test]
    public void Finish_NoGuesses_RateIs0()
    {
        // Arrange
        var flashCard = GetFlashCard();
        var options = GetOptions();
        var session = GetStartedSession([flashCard], options);

        // Act
        session.Finish(options, _timeProvider);

        // Assert
        Assert.That(flashCard.Rate, Is.EqualTo(0));
    }

    private static LearningSession GetStartedSession(FlashCard[] flashCards, LearningSessionOptions options)
    {
        var session = LearningSession.Create(flashCards, _user, options.Exercises);
        session.Start(TimeSpan.FromMinutes(1));

        return session;
    }

    private static FlashCard GetFlashCard() => FlashCard.Create("word1", "слово1", null, _user, _timeProvider);

    private static LearningSessionOptions GetOptions()
    {
        return new()
        {
            RepeatFlashCardPeriod = TimeSpan.FromMinutes(5),
            Exercises =
            [
                ExerciseType.TranslateForeignToNative,
                ExerciseType.TranslateNativeToForeign
            ]
        };
    }
}
