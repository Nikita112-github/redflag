using RedFlag.Core.Privacy;
using Xunit;

namespace RedFlag.Tests;

public class TextHasherTests
{
    [Fact]
    public void Same_text_with_different_spacing_and_case_gives_same_hash()
    {
        var a = TextHasher.Hash("Pay  Rs 100 now");
        var b = TextHasher.Hash("  pay rs 100 NOW ");

        Assert.Equal(a, b);
    }

    [Fact]
    public void Different_text_gives_different_hash()
    {
        Assert.NotEqual(TextHasher.Hash("Pay Rs 100"), TextHasher.Hash("Pay Rs 200"));
    }

    [Fact]
    public void Hash_is_64_characters_long()
    {
        Assert.Equal(64, TextHasher.Hash("hello").Length);
    }
}