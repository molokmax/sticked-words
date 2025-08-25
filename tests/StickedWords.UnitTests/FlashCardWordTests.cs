using StickedWords.Domain.Models;

namespace StickedWords.UnitTests;

public class FlashCardWordTests
{
    [TestCase(" Hello", "Hello", true)]
    [TestCase("Hello ", "Hello", true)]
    [TestCase(" Hello ", "Hello", true)]
    [TestCase("Hello", "hello", true)]
    [TestCase("  HELLO ", "hello", true)]
    [TestCase("hello", "helloo", false)]
    public void EqualsShouldNotConsiderSpacesAndCase(string text1, string text2, bool result)
    {
        var word1 = FlashCardWord.Create(text1);
        var word2 = FlashCardWord.Create(text2);

        var isEqual = word1 == word2;

        Assert.That(isEqual, Is.EqualTo(result));
    }
}
