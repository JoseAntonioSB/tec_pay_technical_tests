

using Catalog.Application.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {request.Id} not found.");
        }

        await _unitOfWork.Products.DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

    }

  
}