

using Catalog.Application.Features.Categories.Commands.CreateCategory;
using Catalog.Application.Features.Categories.Commands.DeleteCategory;
using Catalog.Application.Features.Categories.Commands.UpdateCategory;
using Catalog.Application.Features.Categories.Queries.GetCategories;
using Catalog.Application.Features.Queries.GetCategoryById;
using MediatR;

namespace Catalog.Api.Endpoints;

public static class CategoryEndpoints
{
    public static RouteGroupBuilder MapCategoryEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", Getall).WithName("GetAllCategories");
        group.MapGet("/{id:guid}", GetById).WithName("GetCategoryById");
        group.MapPost("/", Create).WithName("CreateCategory");
        group.MapPut("/{id:guid}", Update).WithName("UpdateCategory");
        group.MapDelete("/{id:guid}", Delete).WithName("DeleteCategory");
        return group;
    }

    private static async Task<IResult> Getall(IMediator mediator, CancellationToken cancellationToken)
    {
        var categories = await mediator.Send(new GetCategoriesQuery(), cancellationToken);
        return Results.Ok(categories);
    }

    private static async Task<IResult> GetById(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Results.Ok(category);
    }

    private static async Task<IResult> Create(CreateCategoryCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        var category = await mediator.Send(command, cancellationToken);
        return Results.CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
    }

    private static async Task<IResult> Update(Guid id, UpdateCategoryCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return Results.BadRequest("Id in the URL does not match Id in the request body.");
        }
        var cmd = command with { Id = id };
        var category = await mediator.Send(cmd, cancellationToken);
        return Results.Ok(category);
    }

    private static async Task<IResult> Delete(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return Results.NoContent();
    }

}