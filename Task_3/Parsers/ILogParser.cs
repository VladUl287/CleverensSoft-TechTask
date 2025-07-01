namespace Task_3.Parsers;

public interface ILogParser
{
    bool TryStandardizeLogLine(string line, out string result);
}
