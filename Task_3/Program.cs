using Task_3;
using Task_3.Parsers;
using Task_3.Formatters;

var formatter = new LogFormatter();
var parsers = new ILogParser[] {
    new ShortDateLogParser(formatter),
    new IsoDateLogParser(formatter)
};
var context = new StandardizeContext(parsers);

while (true)
{
    Console.Write("Input path: ");
    var inputPath = Console.ReadLine();
    ArgumentException.ThrowIfNullOrEmpty(inputPath);
    ArgumentException.ThrowIfNullOrWhiteSpace(inputPath);

    Console.Write("Output path: ");
    var successOutputPath = Console.ReadLine();
    ArgumentException.ThrowIfNullOrEmpty(successOutputPath);
    ArgumentException.ThrowIfNullOrWhiteSpace(successOutputPath);

    var outputDirectory = Path.GetDirectoryName(successOutputPath);
    ArgumentNullException.ThrowIfNull(outputDirectory);

    var problemsOutputPath = Path.Combine(outputDirectory, "problems.txt");

    using var reader = new StreamReader(inputPath);
    using var successWriter = new StreamWriter(successOutputPath);
    using var problemsWriter = new StreamWriter(problemsOutputPath);
    var line = reader.ReadLine();
    while (line is not null)
    {
        var result = context.StandardizeLogLine(line);
        result.Switch(
            (success) => successWriter.WriteLine(success.Value),
            (error) => problemsWriter.WriteLine(error.Value)
        );
        line = reader.ReadLine();
    }

    Console.WriteLine("Finished");
    Console.WriteLine();
}