using OneOf;
using OneOf.Types;
using Task_3.Parsers;

namespace Task_3;

public sealed class StandardizeContext(IEnumerable<ILogParser> parsers)
{
    public OneOf<Success<string>, Error<string>> StandardizeLogLine(string line)
    {
        foreach (var parser in parsers)
        {
            if (parser.TryStandardizeLogLine(line, out var result))
            {
                return new Success<string>(result);
            }
        }
        return new Error<string>(line);
    }
}
