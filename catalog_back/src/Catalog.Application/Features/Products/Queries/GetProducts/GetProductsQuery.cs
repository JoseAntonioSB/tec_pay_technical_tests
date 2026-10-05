
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(
        string? Search,
        string? Name,
        string? Description,
        bool? IsActive,
        Guid? CategoryId,
        int Page = 1,
        int PageSize = 3
) : IRequest<PagedResult<ProductDto>>;