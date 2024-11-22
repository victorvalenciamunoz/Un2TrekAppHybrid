using System.ComponentModel.DataAnnotations;

namespace Un2TrekApp.Authentication;

internal class RegistrationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(5)]
    public string Password { get; set; } 

    [Required]
    [MinLength(3)]
    public string Name { get; set; }

    [Required]
    [MinLength(3)]
    public string LastName { get; set; }

    public bool ReceivePromotionalEmails { get; set; } = false;
}
