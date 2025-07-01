using FluentAssertions;
using Moq;
using OneOf.Types;
using Task_3.Formatters;
using Task_3.Parsers;

namespace Task_3.Tests;

[TestClass]
public class IsoDateLogParserTests
{
    private readonly Mock<ILogFormatter> _mockFormatter = new();
    private IsoDateLogParser _parser;

    [TestInitialize]
    public void Setup()
    {
        _parser = new IsoDateLogParser(_mockFormatter.Object);
    }

    [TestMethod]
    public void TryStandardizeLogLine_ValidFormat_CallsFormatterAndReturnsTrue()
    {
        // Arrange
        const string testLine = "2023-01-01 12:00:00.1234|INFO|123|Main|Test message";
        const string expectedFormatted = "2023-01-01 12:00:00.1234 INFO Test message Main";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "2023-01-01", "12:00:00.1234", "INFO", "Test message", "Main"))
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
        const string testLine = "2023-12-31 23:59:59.9999|ERROR|456|ProcessRequest|Failed to process: {id: 123, user: 'test'}";
        const string expectedFormatted = "2023-12-31 23:59:59.9999 [ERROR] Failed to process: {id: 123, user: 'test'} (ProcessRequest)";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "2023-12-31", "23:59:59.9999", "ERROR", "Failed to process: {id: 123, user: 'test'}", "ProcessRequest"))
            .Returns(new Success<string>(expectedFormatted));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeTrue();
        standardized.Should().Be(expectedFormatted);
    }

    [TestMethod]
    public void TryStandardizeLogLine_ValidFormatWithWhitespaceInMethod_TrimsAndCallsFormatter()
    {
        // Arrange
        const string testLine = "2023-01-01 12:00:00.1234|INFO|123|  Main  |Test message";
        const string expectedFormatted = "2023-01-01 12:00:00.1234 [INFO] Test message (Main)";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "2023-01-01", "12:00:00.1234", "INFO", "Test message", "Main"))
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
        const string testLine = "01-01-2023 12:00:00.1234|INFO|123|Main|Wrong date format";

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
        _mockFormatter.Verify(x => x.FormatLogEntry(It.IsAny<string>(), It.IsAny<string>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public void TryStandardizeLogLine_MissingPipeDelimiter_ReturnsFalse()
    {
        // Arrange
        const string testLine = "2023-01-01 12:00:00.1234 INFO 123 Main Test message";

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
        const string testLine = "2023-01-01 12:00:00.1234|INFO|123|Main|";

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
        const string testLine = "2023-01-01 12:00:00.1234|INFO|123|Main|Test message";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "2023-01-01", "12:00:00.1234", "INFO", "Test message", "Main"))
            .Returns(new Error<string>("Formatting error"));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }

    [TestMethod]
    public void TryStandardizeLogLine_EmptyMethod_ReturnsFalse()
    {
        // Arrange
        const string testLine = "2023-01-01 12:00:00.1234|INFO|123||Test message";

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
        const string testLine = "2020-02-29 00:00:00.0000|DEBUG|789|LeapCheck|Leap year test";
        const string expectedFormatted = "2020-02-29 00:00:00.0000 [DEBUG] Leap year test (LeapCheck)";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "2020-02-29", "00:00:00.0000", "DEBUG", "Leap year test", "LeapCheck"))
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
        const string testLine = "2023-01-01 12:00:00.1234|INFO|123|Main|Test message";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "2023-01-01", "12:00:00.1234", "INFO", "Test message", "Main"))
            .Throws(new Exception("Formatter error"));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeFalse();
        standardized.Should().BeEmpty();
    }

    [TestMethod]
    public void TryStandardizeLogLine_WithAdditionalFieldsAfterMessage_IgnoresExtraFields()
    {
        // Arrange
        const string testLine = "2023-01-01 12:00:00.1234|INFO|123|Main|Test message|extra|fields";
        const string expectedFormatted = "2023-01-01 12:00:00.1234 [INFO] Test message (Main)";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "2023-01-01", "12:00:00.1234", "INFO", "Test message|extra|fields", "Main"))
            .Returns(new Success<string>(expectedFormatted));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeTrue();
        standardized.Should().Be(expectedFormatted);
    }

    [TestMethod]
    public void TryStandardizeLogLine_WithMinimalTimestamp_CorrectlyParses()
    {
        // Arrange
        const string testLine = "2023-01-01 00:00:00.0000|DEBUG|0|Init|Starting up";
        const string expectedFormatted = "2023-01-01 00:00:00.0000 [DEBUG] Starting up (Init)";

        _mockFormatter.Setup(x => x.FormatLogEntry(
            "2023-01-01", "00:00:00.0000", "DEBUG", "Starting up", "Init"))
            .Returns(new Success<string>(expectedFormatted));

        // Act
        var result = _parser.TryStandardizeLogLine(testLine, out var standardized);

        // Assert
        result.Should().BeTrue();
        standardized.Should().Be(expectedFormatted);
    }
}