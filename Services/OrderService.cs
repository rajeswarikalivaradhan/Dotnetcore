using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Order;
using WebApi.Models;

namespace WebApi.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<OrderReadDto>> GetAllAsync()
    {
        return await _db.Orders
            .Include(o => o.Customer)
            .Select(o => new OrderReadDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.Name,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Notes = o.Notes
            })
            .ToListAsync();
    }

    public async Task<OrderReadDto?> GetByIdAsync(int id)
    {
        var o = await _db.Orders.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == id);
        return o == null ? null : new OrderReadDto
        {
            Id = o.Id,
            OrderNumber = o.OrderNumber,
            CustomerId = o.CustomerId,
            CustomerName = o.Customer.Name,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            Notes = o.Notes
        };
    }

    public async Task<OrderReadDto> CreateAsync(OrderCreateDto dto)
    {
        var entity = new Order
        {
            OrderNumber = dto.OrderNumber,
            CustomerId = dto.CustomerId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = dto.TotalAmount,
            Status = dto.Status,
            Notes = dto.Notes
        };
        _db.Orders.Add(entity);
        await _db.SaveChangesAsync();
        var withCustomer = await _db.Orders.Include(x => x.Customer).FirstAsync(x => x.Id == entity.Id);
        return new OrderReadDto
        {
            Id = withCustomer.Id,
            OrderNumber = withCustomer.OrderNumber,
            CustomerId = withCustomer.CustomerId,
            CustomerName = withCustomer.Customer.Name,
            OrderDate = withCustomer.OrderDate,
            TotalAmount = withCustomer.TotalAmount,
            Status = withCustomer.Status,
            Notes = withCustomer.Notes
        };
    }

    public async Task<OrderReadDto?> UpdateAsync(int id, OrderUpdateDto dto)
    {
        var entity = await _db.Orders.FindAsync(id);
        if (entity == null) return null;
        entity.OrderNumber = dto.OrderNumber;
        entity.CustomerId = dto.CustomerId;
        entity.TotalAmount = dto.TotalAmount;
        entity.Status = dto.Status;
        entity.Notes = dto.Notes;
        await _db.SaveChangesAsync();
        var withCustomer = await _db.Orders.Include(x => x.Customer).FirstAsync(x => x.Id == entity.Id);
        return new OrderReadDto
        {
            Id = withCustomer.Id,
            OrderNumber = withCustomer.OrderNumber,
            CustomerId = withCustomer.CustomerId,
            CustomerName = withCustomer.Customer.Name,
            OrderDate = withCustomer.OrderDate,
            TotalAmount = withCustomer.TotalAmount,
            Status = withCustomer.Status,
            Notes = withCustomer.Notes
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Orders.FindAsync(id);
        if (entity == null) return false;
        _db.Orders.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
