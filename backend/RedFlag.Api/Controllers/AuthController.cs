using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedFlag.Api.Models;
using RedFlag.Core.Security;
using RedFlag.Core.Services;

namespace RedFlag.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly ITokenService _tokens;

    public AuthController(IAuthService auth, ITokenService tokens)
    {
        _auth = auth;
        _tokens = tokens;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _auth.RegisterAsync(request.Email, request.Password, cancellationToken);
        if (!result.Success)
        {
            return Conflict(new { error = result.Error });
        }

        return StatusCode(StatusCodes.Status201Created, new { email = result.User!.Email, role = result.User.Role });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _auth.LoginAsync(request.Email, request.Password, cancellationToken);
        if (user == null)
        {
            // The same message for "unknown email" and "wrong password" on purpose.
            return Unauthorized(new { error = "Invalid email or password." });
        }

        var token = _tokens.CreateToken(user);
        return Ok(new AuthResponse
        {
            Token = token.Token,
            ExpiresAtUtc = token.ExpiresAtUtc,
            Email = user.Email,
            Role = user.Role
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            id = User.FindFirstValue("sub"),
            email = User.FindFirstValue("email"),
            role = User.FindFirstValue("role")
        });
    }
}