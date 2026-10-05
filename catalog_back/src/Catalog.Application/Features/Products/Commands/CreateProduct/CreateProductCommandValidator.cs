
using FluentValidation;

namespace Catalog.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
        RuleFor(x=> x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock must be greater than or equal to zero.");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required.");
    }
}