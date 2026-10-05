using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces;

public interface ICategoryRepository
{
    Task<Category> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync(bool? isActive = null, CancellationToken cancellationToken = default);
    Task<Category> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
}
