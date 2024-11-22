using System.ComponentModel.DataAnnotations;

namespace Un2TrekApp.Authentication;

internal class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(5)]
    public string Password { get; set; }
}
