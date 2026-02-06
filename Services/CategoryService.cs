using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs.Category;
using WebApi.Models;

namespace WebApi.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CategoryReadDto>> GetAllAsync()
    {
        return await _db.Categories
            .Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive
            })
            .ToListAsync();
    }

    public async Task<CategoryReadDto?> GetByIdAsync(int id)
    {
        var c = await _db.Categories.FindAsync(id);
        return c == null ? null : new CategoryReadDto
        {
            Id = c.Id,
            Name = c.Name,
            IsActive = c.IsActive
        };
    }

    public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto dto)
    {
        var entity = new Category
        {
            Name = dto.Name,
            IsActive = dto.IsActive
        };
        _db.Categories.Add(entity);
        await _db.SaveChangesAsync();
        return new CategoryReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IsActive = entity.IsActive
        };
    }

    public async Task<CategoryReadDto?> UpdateAsync(int id, CategoryUpdateDto dto)
    {
        var entity = await _db.Categories.FindAsync(id);
        if (entity == null) return null;
        entity.Name = dto.Name;
        entity.IsActive = dto.IsActive;
        await _db.SaveChangesAsync();
        return new CategoryReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IsActive = entity.IsActive
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Categories.FindAsync(id);
        if (entity == null) return false;
        _db.Categories.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
