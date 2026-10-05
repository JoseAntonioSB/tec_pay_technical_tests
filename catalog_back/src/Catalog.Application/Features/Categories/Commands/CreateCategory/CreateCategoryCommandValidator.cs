

using Catalog.Application.Interfaces;
using FluentValidation;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .MustAsync(BeUniqueName).WithMessage("A category with the same name already exists.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryRepository.GetByNameAsync(name);
        return existingCategory == null;
    }
}