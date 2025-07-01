using System.Text;

namespace Task_1;

public static class Decompressor
{
    public static string Decompress(string input)
    {
        ArgumentException.ThrowIfNullOrEmpty(input);

        var result = new StringBuilder();
        var i = 0;

        while (i < input.Length)
        {
            char currentChar = input[i++];

            var countDigits = new StringBuilder();
            while (i < input.Length && char.IsDigit(input[i]))
            {
                countDigits.Append(input[i]);
                i++;
            }

            var count = countDigits.Length > 0 ? int.Parse(countDigits.ToString()) : 1;

            result.Append(currentChar, count);
        }

        return result.ToString();
    }

    public static string Decompress_Optimized(string input)
    {
        ArgumentException.ThrowIfNullOrEmpty(input);
        return Decompress(input.AsSpan());
    }

    public static string Decompress(ReadOnlySpan<char> input)
    {
        var result = new StringBuilder();
        var index = 0;
        while (index < input.Length)
        {
            var currentChar = input[index++];
            var numStart = index;

            while (index < input.Length && char.IsDigit(input[index]))
            {
                index++;
            }

            var count = (index > numStart) ? int.Parse(input[numStart..index]) : 1;
            result.Append(currentChar, count);
        }

        return result.ToString();
    }
}
