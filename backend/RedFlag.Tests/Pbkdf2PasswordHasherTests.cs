using RedFlag.Core.Security;
using Xunit;

namespace RedFlag.Tests;

public class Pbkdf2PasswordHasherTests
{
    // A low iteration count keeps the tests fast. The real app uses the default (600,000).
    private readonly Pbkdf2PasswordHasher _hasher = new(1000);

    [Fact]
    public void Correct_password_verifies()
    {
        var hash = _hasher.Hash("StrongPass123");

        Assert.True(_hasher.Verify("StrongPass123", hash));
    }

    [Fact]
    public void Wrong_password_does_not_verify()
    {
        var hash = _hasher.Hash("StrongPass123");

        Assert.False(_hasher.Verify("strongpass123", hash));
    }

    [Fact]
    public void Same_password_hashed_twice_gives_different_hashes()
    {
        var first = _hasher.Hash("StrongPass123");
        var second = _hasher.Hash("StrongPass123");

        Assert.NotEqual(first, second);
        Assert.StartsWith("v1.", first);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-a-hash")]
    [InlineData("v1.abc.def.ghi")]
    [InlineData("v1.1000.@@@.@@@")]
    public void Malformed_hash_does_not_verify(string storedHash)
    {
        Assert.False(_hasher.Verify("StrongPass123", storedHash));
    }
}