

using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name, string Description) : IRequest<CategoryDto>;