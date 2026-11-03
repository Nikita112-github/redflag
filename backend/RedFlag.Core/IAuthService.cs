using RedFlag.Core.Entities;

namespace RedFlag.Core.Services;

public record RegisterResult(bool Success, User? User, string? Error);

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}