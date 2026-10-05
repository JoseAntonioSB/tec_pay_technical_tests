using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name, string Description, bool IsActive) : IRequest<CategoryDto>;