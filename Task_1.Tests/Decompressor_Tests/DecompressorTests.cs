using FluentAssertions;

namespace Task_1.Tests.Decompressor_Tests;

[TestClass]
public sealed class DecompressorTests
{
    [TestMethod]
    public void BasicDecompression()
    {
        Decompressor.Decompress("a3b2c3d2e")
            .Should().Be("aaabbcccdde");
    }

    [TestMethod]
    public void SingleCharacterWithoutCount()
    {
        Decompressor.Decompress("abc")
            .Should().Be("abc");
    }

    [TestMethod]
    public void MixedSingleAndMultiChar()
    {
        Decompressor.Decompress("a1b2c3")
            .Should().Be("abbccc");
    }

    [TestMethod]
    public void MultiDigitCounts()
    {
        Decompressor.Decompress("a10b20")
            .Should().Be("aaaaaaaaaabbbbbbbbbbbbbbbbbbbb");
    }

    [TestMethod]
    public void SingleCharacterWithCount()
    {
        Decompressor.Decompress("z5")
            .Should().Be("zzzzz");
    }

    [TestMethod]
    public void ConsecutiveNonDigits()
    {
        Decompressor.Decompress("xy3z2")
            .Should().Be("xyyyzz");
    }

    [TestMethod]
    public void LargeCountValue()
    {
        var result = Decompressor.Decompress("a999");
        result.Should().HaveLength(999);
        result.Should().Match(s => s.All(c => c == 'a'));
    }

    [TestMethod]
    public void MixedCaseCharacters()
    {
        Decompressor.Decompress("A2b3C1d")
            .Should().Be("AAbbbCd");
    }

    [TestMethod]
    public void SpecialCharacters()
    {
        Decompressor.Decompress("*3!2@1")
            .Should().Be("***!!@");
    }

    [TestMethod]
    public void Should_Throw_When_NullInput()
    {
        Action act = () => Decompressor.Decompress(null);
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Should_Throw_When_EmptyInput()
    {
        Action act = () => Decompressor.Decompress("");
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Should_Throw_When_CountOverflow()
    {
        Action act = () => Decompressor.Decompress("a99999999999999999999");
        act.Should().Throw<OverflowException>();
    }
}
