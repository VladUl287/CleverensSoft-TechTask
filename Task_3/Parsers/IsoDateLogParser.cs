using System.Text.RegularExpressions;
using Task_3.Formatters;

namespace Task_3.Parsers;

public sealed partial class IsoDateLogParser(ILogFormatter formatter) : ILogParser
{
    public bool TryStandardizeLogLine(string line, out string result)
    {
		try
		{
            // Try Format: "yyyy-MM-dd HH:mm:ss.ffff|LEVEL|...|Method|Message"
            var match = SecondFormatRegex().Match(line);

            if (match.Success)
            {
                var formatResult = formatter.FormatLogEntry(
                    date: match.Groups[1].Value,
                    time: match.Groups[2].Value,
                    level: match.Groups[3].Value,
                    method: match.Groups[4].Value.Trim(),
                    message: match.Groups[5].Value
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

    [GeneratedRegex(@"^(\d{4}-\d{2}-\d{2}) (\d{2}:\d{2}:\d{2}\.\d+)\|(\w+)\|\d+\|([^\|]+)\|(.+)$", RegexOptions.Compiled, 1000)]
    private static partial Regex SecondFormatRegex();
}
