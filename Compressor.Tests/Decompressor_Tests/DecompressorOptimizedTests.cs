using FluentAssertions;

namespace Task_1.Tests.Decompressor_Tests;

[TestClass]
public sealed class DecompressorOptimizedTests
{
    [TestMethod]
    public void BasicDecompression()
    {
        Decompressor.Decompress_Optimized("a3b2c3d2e")
            .Should().Be("aaabbcccdde");
    }

    [TestMethod]
    public void SingleCharacterWithoutCount()
    {
        Decompressor.Decompress_Optimized("abc")
            .Should().Be("abc");
    }

    [TestMethod]
    public void MixedSingleAndMultiChar()
    {
        Decompressor.Decompress_Optimized("a1b2c3")
            .Should().Be("abbccc");
    }

    [TestMethod]
    public void MultiDigitCounts()
    {
        Decompressor.Decompress_Optimized("a10b20")
            .Should().Be("aaaaaaaaaabbbbbbbbbbbbbbbbbbbb");
    }

    [TestMethod]
    public void SingleCharacterWithCount()
    {
        Decompressor.Decompress_Optimized("z5")
            .Should().Be("zzzzz");
    }

    [TestMethod]
    public void ConsecutiveNonDigits()
    {
        Decompressor.Decompress_Optimized("xy3z2")
            .Should().Be("xyyyzz");
    }

    [TestMethod]
    public void LargeCountValue()
    {
        var result = Decompressor.Decompress_Optimized("a999");
        result.Should().HaveLength(999);
        result.Should().Match(s => s.All(c => c == 'a'));
    }

    [TestMethod]
    public void MixedCaseCharacters()
    {
        Decompressor.Decompress_Optimized("A2b3C1d")
            .Should().Be("AAbbbCd");
    }

    [TestMethod]
    public void SpecialCharacters()
    {
        Decompressor.Decompress_Optimized("*3!2@1")
            .Should().Be("***!!@");
    }

    [TestMethod]
    public void Should_Throw_When_NullInput()
    {
        Action act = () => Decompressor.Decompress_Optimized(null);
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Should_Throw_When_EmptyInput()
    {
        Action act = () => Decompressor.Decompress_Optimized("");
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Should_Throw_When_CountOverflow()
    {
        Action act = () => Decompressor.Decompress_Optimized("a99999999999999999999");
        act.Should().Throw<OverflowException>();
    }
}
