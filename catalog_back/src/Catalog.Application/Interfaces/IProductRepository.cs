using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<Product> GetByNameAsync(string name);
    Task<int> GetCountAsync(string? search, string? name, string? description, bool? isActive, Guid? categoryId);
    Task<IEnumerable<Product>> GetAllAsync(string? search, string? name, string? description, bool? isActive, Guid? categoryId, int page, int pageSize);
}