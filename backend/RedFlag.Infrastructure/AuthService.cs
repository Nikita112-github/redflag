using Microsoft.EntityFrameworkCore;
using RedFlag.Core.Entities;
using RedFlag.Core.Security;
using RedFlag.Core.Services;
using RedFlag.Infrastructure.Data;

namespace RedFlag.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly RedFlagDbContext _db;
    private readonly IPasswordHasher _hasher;

    public AuthService(RedFlagDbContext db, IPasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    public async Task<RegisterResult> RegisterAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = Normalize(email);

        if (await _db.Users.AnyAsync(u => u.Email == normalizedEmail, cancellationToken))
        {
            return new RegisterResult(false, null, "An account with this email already exists.");
        }

        var user = new User
        {
            Email = normalizedEmail,
            PasswordHash = _hasher.Hash(password),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);
        return new RegisterResult(true, user, null);
    }

    public async Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = Normalize(email);
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return _hasher.Verify(password, user.PasswordHash) ? user : null;
    }

    private static string Normalize(string email) => email.Trim().ToLowerInvariant();
}