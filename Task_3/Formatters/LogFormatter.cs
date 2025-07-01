using OneOf;
using OneOf.Types;
using System.Collections.Frozen;
using System.Globalization;

namespace Task_3.Formatters;

public sealed class LogFormatter : ILogFormatter
{
    private static readonly FrozenDictionary<string, string> LogLevelMap =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["INFORMATION"] = "INFO",
            ["WARNING"] = "WARN",
            ["INFO"] = "INFO",
            ["WARN"] = "WARN",
            ["ERROR"] = "ERROR",
            ["DEBUG"] = "DEBUG"
        }
        .ToFrozenDictionary();

    public OneOf<Success<string>, Error<string>> FormatLogEntry(string date, string time, string level, string message, string method)
    {
        if (!TryFormatDate(date, out date))
        {
            return new Error<string>("Not correct date format. Only 'dd.MM.yyyy', 'yyyy-MM-dd' allowed.");
        }
        if (!LogLevelMap.TryGetValue(level, out var standardizedLevel))
        {
            return new Error<string>($"Invalid log level: {level}.");
        }

        return new Success<string>($"{date}\t{time}\t{standardizedLevel}\t{method}\t{message}");
    }

    private static bool TryFormatDate(string inputDate, out string formattedDate)
    {
        if (DateTime.TryParseExact(inputDate, "dd.MM.yyyy", null, DateTimeStyles.None, out var parsedDate) ||
            DateTime.TryParseExact(inputDate, "yyyy-MM-dd", null, DateTimeStyles.None, out parsedDate))
        {
            formattedDate = parsedDate.ToString("dd-MM-yyyy");
            return true;
        }

        formattedDate = string.Empty;
        return false;
    }
}
