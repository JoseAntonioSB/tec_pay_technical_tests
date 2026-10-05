
using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    bool IsActive,
    Guid CategoryId
) : IRequest<ProductDto>;