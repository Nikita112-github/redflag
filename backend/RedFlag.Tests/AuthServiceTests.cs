using Microsoft.EntityFrameworkCore;
using RedFlag.Core.Security;
using RedFlag.Infrastructure.Data;
using RedFlag.Infrastructure.Services;
using Xunit;

namespace RedFlag.Tests;

public class AuthServiceTests
{
    private static RedFlagDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<RedFlagDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RedFlagDbContext(options);
    }

    private static AuthService CreateService(RedFlagDbContext db) =>
        new(db, new Pbkdf2PasswordHasher(1000));

    [Fact]
    public async Task Register_stores_normalized_email_hashed_password_and_user_role()
    {
        using var db = CreateDb();
        var service = CreateService(db);

        var result = await service.RegisterAsync("  Nikita@Example.com ", "StrongPass123");

        Assert.True(result.Success);
        Assert.NotNull(result.User);
        Assert.Equal("nikita@example.com", result.User!.Email);
        Assert.Equal("User", result.User.Role);
        Assert.NotEqual("StrongPass123", result.User.PasswordHash);
        Assert.StartsWith("v1.", result.User.PasswordHash);
    }

    [Fact]
    public async Task Register_rejects_a_duplicate_email_ignoring_case()
    {
        using var db = CreateDb();
        var service = CreateService(db);
        await service.RegisterAsync("a@example.com", "StrongPass123");

        var second = await service.RegisterAsync("A@EXAMPLE.COM", "AnotherPass456");

        Assert.False(second.Success);
        Assert.Null(second.User);
        Assert.Equal(1, await db.Users.CountAsync());
    }

    [Fact]
    public async Task Login_succeeds_with_the_correct_password_ignoring_email_case()
    {
        using var db = CreateDb();
        var service = CreateService(db);
        await service.RegisterAsync("a@example.com", "StrongPass123");

        var user = await service.LoginAsync("A@Example.com", "StrongPass123");

        Assert.NotNull(user);
        Assert.Equal("a@example.com", user!.Email);
    }

    [Fact]
    public async Task Login_fails_with_a_wrong_password()
    {
        using var db = CreateDb();
        var service = CreateService(db);
        await service.RegisterAsync("a@example.com", "StrongPass123");

        Assert.Null(await service.LoginAsync("a@example.com", "WrongPass999"));
    }

    [Fact]
    public async Task Login_fails_for_an_unknown_email()
    {
        using var db = CreateDb();
        var service = CreateService(db);

        Assert.Null(await service.LoginAsync("nobody@example.com", "StrongPass123"));
    }
}