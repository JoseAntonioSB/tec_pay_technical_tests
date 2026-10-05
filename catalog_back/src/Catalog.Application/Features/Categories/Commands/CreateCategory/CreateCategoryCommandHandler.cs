using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var exist = await _unitOfWork.Categories.GetByNameAsync(request.Name);
        if (exist != null)
        {
            throw new InvalidOperationException("Category already exists.");
        }

        var category = Category.Create(request.Name, request.Description);
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(category);
    }
}