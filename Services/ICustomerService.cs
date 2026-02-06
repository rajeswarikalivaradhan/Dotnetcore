using WebApi.DTOs.Customer;

namespace WebApi.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerReadDto>> GetAllAsync();
    Task<CustomerReadDto?> GetByIdAsync(int id);
    Task<CustomerReadDto> CreateAsync(CustomerCreateDto dto);
    Task<CustomerReadDto?> UpdateAsync(int id, CustomerUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
