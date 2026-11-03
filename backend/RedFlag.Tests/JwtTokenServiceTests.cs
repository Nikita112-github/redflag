using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using RedFlag.Api.Security;
using RedFlag.Core.Entities;
using Xunit;

namespace RedFlag.Tests;

public class JwtTokenServiceTests
{
    private static JwtSettings TestSettings() => new()
    {
        Key = "this-is-a-test-key-with-more-than-32-characters",
        Issuer = "redflag-tests",
        Audience = "redflag-tests",
        ExpiryMinutes = 30
    };

    [Fact]
    public void Token_contains_user_id_email_role_and_issuer()
    {
        var service = new JwtTokenService(Options.Create(TestSettings()));
        var user = new User { Id = 7, Email = "admin@example.com", Role = "Admin" };

        var result = service.CreateToken(user);
        var parsed = new JsonWebTokenHandler().ReadJsonWebToken(result.Token);

        Assert.Contains(parsed.Claims, c => c.Type == "sub" && c.Value == "7");
        Assert.Contains(parsed.Claims, c => c.Type == "email" && c.Value == "admin@example.com");
        Assert.Contains(parsed.Claims, c => c.Type == "role" && c.Value == "Admin");
        Assert.Equal("redflag-tests", parsed.Issuer);
    }

    [Fact]
    public void Token_expires_in_the_future()
    {
        var service = new JwtTokenService(Options.Create(TestSettings()));
        var user = new User { Id = 1, Email = "a@example.com", Role = "User" };

        var result = service.CreateToken(user);
        var parsed = new JsonWebTokenHandler().ReadJsonWebToken(result.Token);

        Assert.True(result.ExpiresAtUtc > DateTime.UtcNow);
        Assert.True(parsed.ValidTo > DateTime.UtcNow);
    }

    [Fact]
    public void A_short_key_is_rejected()
    {
        var settings = TestSettings();
        settings.Key = "too-short";

        Assert.Throws<InvalidOperationException>(() => new JwtTokenService(Options.Create(settings)));
    }
}