
using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery(bool? IsActive = null) : IRequest<IEnumerable<CategoryDto>>;