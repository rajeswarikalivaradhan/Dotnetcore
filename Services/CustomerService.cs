using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Customer;
using WebApi.Models;

namespace WebApi.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _db;

    public CustomerService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CustomerReadDto>> GetAllAsync()
    {
        return await _db.Customers
            .Select(c => new CustomerReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Mobile = c.Mobile
            })
            .ToListAsync();
    }

    public async Task<CustomerReadDto?> GetByIdAsync(int id)
    {
        var c = await _db.Customers.FindAsync(id);
        return c == null ? null : new CustomerReadDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Mobile = c.Mobile
        };
    }

    public async Task<CustomerReadDto> CreateAsync(CustomerCreateDto dto)
    {
        var entity = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            Mobile = dto.Mobile
        };
        _db.Customers.Add(entity);
        await _db.SaveChangesAsync();
        return new CustomerReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Mobile = entity.Mobile
        };
    }

    public async Task<CustomerReadDto?> UpdateAsync(int id, CustomerUpdateDto dto)
    {
        var entity = await _db.Customers.FindAsync(id);
        if (entity == null) return null;
        entity.Name = dto.Name;
        entity.Email = dto.Email;
        entity.Mobile = dto.Mobile;
        await _db.SaveChangesAsync();
        return new CustomerReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Mobile = entity.Mobile
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Customers.FindAsync(id);
        if (entity == null) return false;
        _db.Customers.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
