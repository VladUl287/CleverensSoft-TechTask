using FluentAssertions;

namespace Task_1.Tests.Compressor_Tests;

[TestClass]
public sealed class CompressorTests
{
    [TestMethod]
    public void Compress_StandardInput_ReturnsCompressedString()
    {
        // Arrange
        string input = "aaabbcccdde";

        // Act
        string result = Compressor.Compress(input);

        // Assert
        result.Should().Be("a3b2c3d2e");
    }

    [TestMethod]
    public void Compress_SingleCharacters_ReturnsCharactersWithoutCount()
    {
        // Arrange
        string input = "abcde";

        // Act
        string result = Compressor.Compress(input);

        // Assert
        result.Should().Be("abcde");
    }

    [TestMethod]
    public void Compress_CaseSensitive_HandlesCaseDifferences()
    {
        // Arrange
        string input = "aaaAAAbbBB";

        // Act
        string result = Compressor.Compress(input);

        // Assert
        result.Should().Be("a3A3b2B2");
    }

    [TestMethod]
    public void Compress_EmptyString_ThrowsArgumentException()
    {
        // Arrange
        string input = "";

        // Act
        Action act = () => Compressor.Compress(input);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Compress_NullInput_ThrowsArgumentNullException()
    {
        // Arrange
        string input = null;

        // Act
        Action act = () => Compressor.Compress(input);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Compress_NonAlphabeticCharacters_CompressesCorrectly()
    {
        // Arrange
        string input = "111!!!@@@";

        // Act
        string result = Compressor.Compress(input);

        // Assert
        result.Should().Be("13!3@3");
    }

    [TestMethod]
    public void Compress_LongSequence_HandlesLargeCounts()
    {
        // Arrange
        string input = new string('a', 1000);

        // Act
        string result = Compressor.Compress(input);

        // Assert
        result.Should().Be("a1000");
    }
}
