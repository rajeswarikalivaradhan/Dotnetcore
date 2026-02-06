using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Category;

public class CategoryCreateDto
{
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public bool IsActive { get; set; } = true;
}
