
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommandHandler: IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    

    public CreateProductCommandHandler(
           IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
        if (category == null)
        {
            throw new InvalidOperationException($"Category with Id {request.CategoryId} not found.");
        }
        var existingProduct = await _unitOfWork.Products.GetByNameAsync(request.Name);
        if (existingProduct != null)
        {
            throw new InvalidOperationException($"A product with the name '{request.Name}' already exists.");
        }

        var product = Product.Create(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.CategoryId          
        );

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductDto>(product);
    }
}
          