using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Category;

public class CategoryUpdateDto
{
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public bool IsActive { get; set; }
}
