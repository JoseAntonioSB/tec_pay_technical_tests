
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with Id {request.Id} not found.");
        }

        var nameExists = await _unitOfWork.Products.GetByNameAsync(request.Name);
        if (nameExists != null && nameExists.Id != request.Id)
        {
            throw new InvalidOperationException($"Product with name {request.Name} already exists.");
        }

        product.Update(request.Name, request.Description, request.Price, request.IsActive, request.CategoryId);
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductDto>(product);
    }
}