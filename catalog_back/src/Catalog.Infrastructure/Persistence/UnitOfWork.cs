using Catalog.Application.Interfaces;
using Catalog.Infrastructure.Repository;

namespace Catalog.Infrastructure.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public ICategoryRepository Categories {get; private set; }

    public IProductRepository Products {get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Categories = new CatalogRepository(_context);
        Products = new ProductRepository(_context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}