using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    async Task IRequestHandler<DeleteCategoryCommand>.Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category == null)
        {
            throw new InvalidOperationException($"Category with Id {request.Id} not found.");
        }

        category.Deactivate();
        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);   
    }
}