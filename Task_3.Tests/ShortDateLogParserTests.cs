using FluentAssertions;
using Moq;
using OneOf.Types;
using Task_3.Formatters;
using Task_3.Parsers;

namespace Task_3.Tests;

[TestClass]
public class ShortDateLogParserTests
{
    private readonly Mock<ILogFormatter> _mockFormatter = new();
    private ShortDateLogParser _parser;

    [TestInitialize]
    public void Setup()
    {
        _parser = new ShortDateLogParser(_mockFormatter.Object);
    }

    [TestMethod]
    public void TryStandardizeLogLine_ValidFormat_CallsFormatterAndReturnsTrue()
    {
        // Arrange
        const string testLine = "01.01.2023 12:00:00.123 INFO Test message";
        const string expectedFormatted = "2023-01-01 12:00:00.123 [INFO] Test message (DEFAULT)";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "01.01.2023", "12:00:00.123", "INFO", "Test message", "DEFAULT"))
            .Returns(new Success<string>(expectedFormatted));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeTrue();
        standardized.Should().Be(expectedFormatted);
        _mockFormatter.VerifyAll();
    }

    [TestMethod]
    public void TryStandardizeLogLine_ValidFormatWithComplexMessage_CallsFormatterAndReturnsTrue()
    {
        // Arrange
        const string testLine = "31.12.2023 23:59:59.999 ERROR Failed to process request: {id: 123, user: 'test'}";
        const string expectedFormatted = "2023-12-31 23:59:59.999 [ERROR] Failed to process request: {id: 123, user: 'test'} (DEFAULT)";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "31.12.2023", "23:59:59.999", "ERROR", "Failed to process request: {id: 123, user: 'test'}", "DEFAULT"))
            .Returns(new Success<string>(expectedFormatted));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeTrue();
        standardized.Should().Be(expectedFormatted);
    }

    [TestMethod]
    public void TryStandardizeLogLine_InvalidDateFormat_ReturnsFalse()
    {
        // Arrange
        const string testLine = "2023-01-01 12:00:00.123 INFO Wrong date format";

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
        _mockFormatter.Verify(x => x.FormatLogEntry(It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void TryStandardizeLogLine_MissingTimeComponent_ReturnsFalse()
    {
        // Arrange
        const string testLine = "01.01.2023 INFO Missing time component";

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }

    [TestMethod]
    public void TryStandardizeLogLine_EmptyMessage_ReturnsFalse()
    {
        // Arrange
        const string testLine = "01.01.2023 12:00:00.123 INFO ";

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }

    [TestMethod]
    public void TryStandardizeLogLine_FormatterReturnsError_ReturnsFalse()
    {
        // Arrange
        const string testLine = "01.01.2023 12:00:00.123 INFO Test message";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "01.01.2023", "12:00:00.123", "INFO", "Test message", "DEFAULT"))
            .Returns(new Error<string>("Formatting error"));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }

    [TestMethod]
    public void TryStandardizeLogLine_EmptyLine_ReturnsFalse()
    {
        // Arrange
        const string testLine = "";

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }

    [TestMethod]
    public void TryStandardizeLogLine_WhitespaceLine_ReturnsFalse()
    {
        // Arrange
        const string testLine = "   ";

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }

    [TestMethod]
    public void TryStandardizeLogLine_InvalidLevelFormat_ReturnsFalse()
    {
        // Arrange
        const string testLine = "01.01.2023 12:00:00.123 INVALID_LEVEL Test message";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "01.01.2023", "12:00:00.123", "INVALID_LEVEL", "Test message", "DEFAULT"))
            .Returns(new Error<string>("Formatting error"));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }

    [TestMethod]
    public void TryStandardizeLogLine_EdgeCaseDateTime_HandlesCorrectly()
    {
        // Arrange
        const string testLine = "29.02.2020 00:00:00.000 DEBUG Leap year test";
        const string expectedFormatted = "2020-02-29 00:00:00.000 [DEBUG] Leap year test (DEFAULT)";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "29.02.2020", "00:00:00.000", "DEBUG", "Leap year test", "DEFAULT"))
            .Returns(new Success<string>(expectedFormatted));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeTrue();
        standardized.Should().Be(expectedFormatted);
    }

    [TestMethod]
    public void TryStandardizeLogLine_ExceptionInFormatter_ReturnsFalse()
    {
        // Arrange
        const string testLine = "01.01.2023 12:00:00.123 INFO Test message";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "01.01.2023", "12:00:00.123", "INFO", "Test message", "DEFAULT"))
            .Throws(new Exception("Formatter error"));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }
}