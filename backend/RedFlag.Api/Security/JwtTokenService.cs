using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RedFlag.Core.Entities;
using RedFlag.Core.Security;

namespace RedFlag.Api.Security;

public class JwtTokenService : ITokenService
{
    private readonly JwtSettings _settings;
    private readonly SigningCredentials _credentials;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;

        var keyBytes = Encoding.UTF8.GetBytes(_settings.Key);
        if (keyBytes.Length < 32)
        {
            throw new InvalidOperationException(
                "Jwt:Key must be at least 32 characters. Set it with dotnet user-secrets.");
        }

        _credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
    }

    public TokenResult CreateToken(User user)
    {
        var expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Expires = expires,
            SigningCredentials = _credentials,
            Claims = new Dictionary<string, object>
            {
                ["sub"] = user.Id.ToString(),
                ["email"] = user.Email,
                ["role"] = user.Role
            }
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);
        return new TokenResult(token, expires);
    }
}