using Task_1;
using BenchmarkDotNet.Running;
using Task_1.Benchmarks;

var example = "aaabbcccdde";
var compressed = Compressor.Compress_Optimized(example);
Console.WriteLine($"Original: {example}");
Console.WriteLine($"Compressed: {compressed}");
Console.WriteLine($"Decompressed: {Decompressor.Decompress_Optimized(example)}");
Console.WriteLine();

BenchmarkRunner.Run<BenchmarkCompressor>();
//BenchmarkRunner.Run<BenchmarkDecompressor>();
return;