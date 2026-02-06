using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Product;

public class ProductCreateDto
{
    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}
