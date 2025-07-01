using OneOf.Types;
using OneOf;

namespace Task_3.Formatters;

public interface ILogFormatter
{
    OneOf<Success<string>, Error<string>> FormatLogEntry(string date, string time, string level, string message, string method);
}
