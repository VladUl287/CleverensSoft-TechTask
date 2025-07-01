using BenchmarkDotNet.Attributes;

namespace Task_1.Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDecompressor
{
    private const string Input_Decompress_Base = "a3b2c3d2e";
    private const string Input_Decompress_256 = "fpok2ewtlytgxmecahlrkphaxdmumyebxhpdsjsafvpxuzwvqhos2pfyvmgefnenudkpsjasdsblcamshybwqp2hrwdxaejuynjajwzrnpkfreqruwylphm2woeskcwlzypozmgyraxw2sjkwehrbyeyirmxasvnozp2wsltzgvhe2ynirvnmqcetsp2k2ukacyfydrmofmcfdjkbtd2khaoucakdclsrpzwdsvytlwgefywlnjgmkdpq2wnyzot";
    private const string Input_Decompress_512 = "treqbfvbsqnwvilmrsylhubtynmxtfrpyeyalofwjelnxzbapgrfafvkrtlewjoaojspunysmnbmqgmygaypgxufitbdbsjgdmpvlzkrdgcvythwcryrwrfh2olduwnjaj2n2cqutfqutstqtfmsqfianqnpaktvznsumpswxwkgbqpnzuq3aferbovita2kmchaeflkcftcxm2hvgwlcxkcacimhzekbusmk2tqdejlokspmhnlhltiznbkhtlsnlncitxfuhsgybocejkphsrzxtkufpc2b2xuimtqhjacpahdfbsecgcviuncicpygfamxdpmgup2gaiwqtyuwturcvlea2b2gixmhitjdoegtsmvlzlwdbwzxpstywpwysamgrwlhcybhdxqj2bmadgnhjbaonrylditcbqnzmhmyrynchlqbjo2plkwxh2ztirqurvhcosg2mhuwzyujenfsjzqxifdhzv2rq2wyrynrpzpnziwdrsonuxhjkb";

    [Benchmark]
    [BenchmarkCategory("Decompress")]
    public string Decompress_Base() => Decompressor.Decompress(Input_Decompress_Base);
    [Benchmark]
    [BenchmarkCategory("Decompress")]
    public string Decompress_Optimized_Base() => Decompressor.Decompress_Optimized(Input_Decompress_Base);

    [Benchmark]
    [BenchmarkCategory("Decompress")]
    public string Decompress_256() => Decompressor.Decompress(Input_Decompress_256);
    [Benchmark]
    [BenchmarkCategory("Decompress")]
    public string Decompress_Optimized_256() => Decompressor.Decompress_Optimized(Input_Decompress_256);

    [Benchmark]
    [BenchmarkCategory("Decompress")]
    public string Decompress_512() => Decompressor.Decompress(Input_Decompress_512);
    [Benchmark]
    [BenchmarkCategory("Decompress")]
    public string Decompress_Optimized_512() => Decompressor.Decompress_Optimized(Input_Decompress_512);
}
