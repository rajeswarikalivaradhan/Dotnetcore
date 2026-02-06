using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Product;
using WebApi.Models;

namespace WebApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductReadDto>> GetAllAsync()
    {
        return await _db.Products
            .Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description
            })
            .ToListAsync();
    }

    public async Task<ProductReadDto?> GetByIdAsync(int id)
    {
        var p = await _db.Products.FindAsync(id);
        return p == null ? null : new ProductReadDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description
        };
    }

    public async Task<ProductReadDto> CreateAsync(ProductCreateDto dto)
    {
        var entity = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description
        };
        _db.Products.Add(entity);
        await _db.SaveChangesAsync();
        return new ProductReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Description = entity.Description
        };
    }

    public async Task<ProductReadDto?> UpdateAsync(int id, ProductUpdateDto dto)
    {
        var entity = await _db.Products.FindAsync(id);
        if (entity == null) return null;
        entity.Name = dto.Name;
        entity.Price = dto.Price;
        entity.Description = dto.Description;
        await _db.SaveChangesAsync();
        return new ProductReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Description = entity.Description
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Products.FindAsync(id);
        if (entity == null) return false;
        _db.Products.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
