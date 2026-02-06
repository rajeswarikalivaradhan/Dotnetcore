using WebApi.DTOs.Order;

namespace WebApi.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderReadDto>> GetAllAsync();
    Task<OrderReadDto?> GetByIdAsync(int id);
    Task<OrderReadDto> CreateAsync(OrderCreateDto dto);
    Task<OrderReadDto?> UpdateAsync(int id, OrderUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
