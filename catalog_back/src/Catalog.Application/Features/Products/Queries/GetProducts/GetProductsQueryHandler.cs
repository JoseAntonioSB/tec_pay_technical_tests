
using AutoMapper;
using Catalog.Application.Common;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Products.GetAllAsync(
            request.Search,
            request.Name,
            request.Description,
            request.IsActive,
            request.CategoryId,
            request.Page,
            request.PageSize
        );

        var total = await _unitOfWork.Products.GetCountAsync(
            request.Search,
            request.Name,
            request.Description,
            request.IsActive,
            request.CategoryId
        );

        return new PagedResult<ProductDto>
        {
            Items = _mapper.Map<IEnumerable<ProductDto>>(products),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
            
    }
}