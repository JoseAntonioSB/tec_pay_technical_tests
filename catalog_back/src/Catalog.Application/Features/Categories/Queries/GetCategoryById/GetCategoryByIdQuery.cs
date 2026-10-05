
using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto>;