using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Auth;

public class ChangePasswordRequest
{
    [Required]
    [DataType(DataType.Password)]
    public required string CurrentPassword { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    public required string NewPassword { get; set; }
}
