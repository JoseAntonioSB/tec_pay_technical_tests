using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(string Name, string Description, decimal Price, int Stock, Guid CategoryId) : IRequest<ProductDto>;