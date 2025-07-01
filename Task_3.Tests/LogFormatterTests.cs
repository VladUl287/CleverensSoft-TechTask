using FluentAssertions;
using Task_3.Formatters;

namespace Task_3.Tests;

[TestClass]
public sealed class LogFormatterTests
{
    private readonly LogFormatter _formatter = new();

    [TestMethod]
    public void FormatLogEntry_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var date = "2023-01-01";
        var time = "12:00:00";
        var level = "INFO";
        var message = "Test message";
        var method = "TestMethod";

        // Act
        var result = _formatter.FormatLogEntry(date, time, level, message, method);

        // Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.Value.Should().NotBeNullOrWhiteSpace();
        result.AsT0.Value.Should().Contain("01-01-2023\t12:00:00\tINFO\tTestMethod\tTest message");
    }

    [TestMethod]
    public void FormatLogEntry_NotCorrectDateFormat_ReturnsError()
    {
        // Arrange
        var date = "2023/01/01";
        var time = "12:00:00";
        var level = "INFO";
        var message = "Test message";
        var method = "TestMethod";

        // Act
        var result = _formatter.FormatLogEntry(date, time, level, message, method);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Value.Should().Contain("Not correct date format. Only 'dd.MM.yyyy', 'yyyy-MM-dd' allowed.");
    }

    [TestMethod]
    public void FormatLogEntry_NotCorrectLogLevel_ReturnsError()
    {
        // Arrange
        var date = "2023-01-01";
        var time = "12:00:00";
        var level = "CRITICAL";
        var message = "Test message";
        string method = "TestMethod";

        // Act
        var result = _formatter.FormatLogEntry(date, time, level, message, method);

        // Assert
        result.IsT1.Should().BeTrue();
        result.AsT1.Value.Should().Contain("Invalid log level: CRITICAL.");
    }
}
