using System.ComponentModel.DataAnnotations;

namespace RedFlag.Api.Models;

public class LoginRequest
{
    [Required]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
}