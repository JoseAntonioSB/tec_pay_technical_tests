using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repository;
                
public class CatalogRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CatalogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Category> GetByIdAsync(Guid id)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) ?? throw new KeyNotFoundException($"Category with ID {id} not found.");
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<Category> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name, cancellationToken) ?? null;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Categories.AsQueryable();
        if (isActive.HasValue)
        {
            query = query.Where(c => c.IsActive == isActive.Value);
        }
        return await query.ToListAsync(cancellationToken);
    }
}