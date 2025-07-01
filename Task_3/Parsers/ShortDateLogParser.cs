using System.Text.RegularExpressions;
using Task_3.Formatters;

namespace Task_3.Parsers;

public sealed partial class ShortDateLogParser(ILogFormatter formatter) : ILogParser
{
    public bool TryStandardizeLogLine(string line, out string result)
    {
        try
        {
            // Try Format: "dd.MM.yyyy HH:mm:ss.fff LEVEL Message"
            var match = FirstFormatRegex().Match(line);

            if (match.Success)
            {
                const string DEFAULT_METHOD = "DEFAULT";

                var formatResult = formatter.FormatLogEntry(
                    date: match.Groups[1].Value,
                    time: match.Groups[2].Value,
                    level: match.Groups[3].Value,
                    message: match.Groups[4].Value,
                    method: DEFAULT_METHOD
                );

                if (formatResult.IsT0)
                {
                    result = formatResult.AsT0.Value;
                    return true;
                }
            }

            result = string.Empty;
            return false;
        }
        catch
        {
            //log error
            result = string.Empty;
            return false;
        }
    }

    [GeneratedRegex(@"^(\d{2}\.\d{2}\.\d{4}) (\d{2}:\d{2}:\d{2}\.\d+) (\w+) (.+)$", RegexOptions.Compiled, 1000)]
    private static partial Regex FirstFormatRegex();
}
