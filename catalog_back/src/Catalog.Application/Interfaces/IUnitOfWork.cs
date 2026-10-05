namespace Catalog.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }

}