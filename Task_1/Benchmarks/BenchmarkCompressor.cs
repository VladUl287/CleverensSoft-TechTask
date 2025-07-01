using BenchmarkDotNet.Attributes;

namespace Task_1.Benchmarks;

[MemoryDiagnoser]
[CategoriesColumn]
public class BenchmarkCompressor
{
    private const string Input_Base = "aaabbcccdde";
    private const string Input_256 = "fpokkewtlytgxmecahlrkphaxdmumyebxhpdsjsafvpxuzwvqhosspfyvmgefnenudkpsjasdsblcamshybwqpphrwdxaejuynjajwzrnpkfreqruwylphmmwoeskcwlzypozmgyraxwwsjkwehrbyeyirmxasvnozppwsltzgvheeynirvnmqcetsppkkukacyfydrmofmcfdjkbtddkhaoucakdclsrpzwdsvytlwgefywlnjgmkdpqqwnyzot";
    private const string Input_512 = "treqbfvbsqnwvilmrsylhubtynmxtfrpyeyalofwjelnxzbapgrfafvkrtlewjoaojspunysmnbmqgmygaypgxufitbdbsjgdmpvlzkrdgcvythwcryrwrfhholduwnjajjnncqutfqutstqtfmsqfianqnpaktvznsumpswxwkgbqpnzuqqqaferbovitaakmchaeflkcftcxmmhvgwlcxkcacimhzekbusmkktqdejlokspmhnlhltiznbkhtlsnlncitxfuhsgybocejkphsrzxtkufpccbbxuimtqhjacpahdfbsecgcviuncicpygfamxdpmguppgaiwqtyuwturcvleaabbgixmhitjdoegtsmvlzlwdbwzxpstywpwysamgrwlhcybhdxqjjbmadgnhjbaonrylditcbqnzmhmyrynchlqbjooplkwxhhztirqurvhcosggmhuwzyujenfsjzqxifdhzvvrqqwyrynrpzpnziwdrsonuxhjkb";

    [Benchmark]
    [BenchmarkCategory("A", "base")]
    public string Compress_Base() => Compressor.Compress(Input_Base);
    [Benchmark]
    [BenchmarkCategory("A", "base")]
    public string Compress_Optimized_Base() => Compressor.Compress_Optimized(Input_Base);

    [Benchmark]
    [BenchmarkCategory("B", "256")]
    public string Compress_256() => Compressor.Compress(Input_256);
    [Benchmark]
    [BenchmarkCategory("B", "256")]
    public string Compress_Optimized_256() => Compressor.Compress_Optimized(Input_256);

    [Benchmark]
    [BenchmarkCategory("C", "512")]
    public string Compress_512() => Compressor.Compress(Input_512);
    [Benchmark]
    [BenchmarkCategory("C", "512")]
    public string Compress_Optimized_512() => Compressor.Compress_Optimized(Input_512);
}
