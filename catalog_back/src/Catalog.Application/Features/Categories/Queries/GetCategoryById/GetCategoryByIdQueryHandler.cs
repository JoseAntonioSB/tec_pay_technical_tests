

using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Application.Features.Queries.GetCategoryById;
using MediatR;

namespace Catalog.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id) ?? throw new InvalidOperationException($"Category with Id {request.Id} not found.");
        return _mapper.Map<CategoryDto>(category);
    }
}