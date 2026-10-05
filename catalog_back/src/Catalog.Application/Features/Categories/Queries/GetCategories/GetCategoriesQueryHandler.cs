
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Features.Categories.Queries.GetCategories;
using Catalog.Application.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler: IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(request.IsActive, cancellationToken);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}