using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Auth;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
