using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Customer;

public class CustomerUpdateDto
{
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [MaxLength(20)]
    public string? Mobile { get; set; }
}
