

using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id) ??
            throw new KeyNotFoundException($"Product with ID {id} not found.");
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);

    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<Product> GetByNameAsync(string name)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Name == name) ?? null;
    }

    public async Task<int> GetCountAsync(string? search, string? name, string? description, bool? isActive, Guid? categoryId)
    {
        var query = _context.Products.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            var s = search.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(s) || p.Description.ToLower().Contains(s));
        }
        if (!string.IsNullOrEmpty(name))
        {
            var n = name.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(n));
        }
        if (!string.IsNullOrEmpty(description))
        {
            var d = description.ToLower();
            query = query.Where(p => p.Description.ToLower().Contains(d));
        }
        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }
        return await query.CountAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync(string? search, string? name, string? description, bool? isActive, Guid? categoryId, int page, int pageSize)
    {
        // implement search in name and description
        var query = _context.Products.AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            // handle also lowercase and uppercase
            query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()) || p.Description.ToLower().Contains(search.ToLower()));
        }
        if (!string.IsNullOrEmpty(name))
        {
            // handle also lowercase and uppercase
            query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
        }
        if (!string.IsNullOrEmpty(description))
        {
            // handle also lowercase and uppercase
            query = query.Where(p => p.Description.ToLower().Contains(description.ToLower()));
        }
        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }
        // implements pagination 
        var skip = (page - 1) * pageSize;
        var result = await query.Skip(skip).Take(pageSize).ToListAsync();
        return result;
    }
}