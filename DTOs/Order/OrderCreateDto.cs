using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs.Order;

public class OrderCreateDto
{
    [Required]
    [MaxLength(50)]
    public required string OrderNumber { get; set; }

    public int CustomerId { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [MaxLength(500)]
    public string? Notes { get; set; }
}
