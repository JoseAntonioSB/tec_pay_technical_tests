
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category == null)
        {
            throw new InvalidOperationException($"Category with Id {request.Id} not found.");
        }

        category.Update(request.Name, request.Description);
        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CategoryDto>(category);
    }
}