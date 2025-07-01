using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text;

namespace Task_1;

public static class Compressor
{
    public static string Compress(string input)
    {
        ArgumentException.ThrowIfNullOrEmpty(input);

        var compressed = new StringBuilder(input.Length);
        var currentChar = input[0];
        var count = 1;

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == currentChar)
            {
                count++;
                continue;
            }

            compressed.Append(currentChar);
            if (count > 1) compressed.Append(count);

            currentChar = input[i];
            count = 1;
        }

        compressed.Append(currentChar);
        if (count > 1) compressed.Append(count);

        return compressed.ToString();
    }

    public static string Compress_Optimized(string input)
    {
        ArgumentException.ThrowIfNullOrEmpty(input);

        const int MaxStackAlloc = 256;

        if (input.Length <= MaxStackAlloc)
        {
            return CompressStackalloc(input);
        }

        return CompressPooling(input);
    }

    private static string CompressPooling(string input)
    {
        var buffer = ArrayPool<char>.Shared.Rent(input.Length);
        var position = 0;

        var current = input[0];
        var count = 1;
        for (var i = 1; i < input.Length; i++)
        {
            if (input[i] == current)
            {
                count++;
                continue;
            }

            buffer[position++] = current;
            if (count > 1) WriteNumber(buffer, ref position, count);

            current = input[i];
            count = 1;
        }

        buffer[position++] = current;
        if (count > 1) WriteNumber(buffer, ref position, count);

        var result = new string(buffer, 0, position);
        ArrayPool<char>.Shared.Return(buffer);
        return result;
    }

    private static string CompressStackalloc(string input)
    {
        Span<char> buffer = stackalloc char[input.Length];
        var position = 0;

        var current = input[0];
        var count = 1;

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == current)
            {
                count++;
                continue;
            }

            buffer[position++] = current;
            if (count > 1) WriteNumber(buffer, ref position, count);

            current = input[i];
            count = 1;
        }

        buffer[position++] = current;
        if (count > 1) WriteNumber(buffer, ref position, count);

        return new string(buffer[..position]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteNumber(Span<char> buffer, ref int position, int count)
    {
        int start = position;
        while (count > 0)
        {
            buffer[position++] = (char)((count % 10) + '0');
            count /= 10;
        }

        var end = position - 1;
        while (start < end)
        {
            (buffer[end], buffer[start]) = (buffer[start], buffer[end]);
            start++;
            end--;
        }
    }
}
