using RedFlag.Core.Entities;

namespace RedFlag.Core.Security;

public record TokenResult(string Token, DateTime ExpiresAtUtc);

public interface ITokenService
{
    TokenResult CreateToken(User user);
}