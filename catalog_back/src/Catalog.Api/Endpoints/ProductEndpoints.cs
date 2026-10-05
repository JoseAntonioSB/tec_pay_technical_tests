

using Catalog.Application.Features.Products.Commands.CreateProduct;
using Catalog.Application.Features.Products.Commands.DeleteProduct;
using Catalog.Application.Features.Products.Commands.UpdateProduct;
using Catalog.Application.Features.Products.Queries.GetProductById;
using Catalog.Application.Features.Products.Queries.GetProducts;
using MediatR;

namespace Catalog.Api.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAll).WithName("GetAllProducts");
        group.MapGet("/{id:guid}", GetById).WithName("GetProductById");
        group.MapPost("/", Create).WithName("CreateProduct");
        group.MapPut("/{id:guid}", Update).WithName("UpdateProduct");
        group.MapDelete("/{id:guid}", Delete).WithName("DeleteProduct");
        return group;
    }

    private static async Task<IResult> GetAll(
        IMediator mediator,
        [AsParameters] GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        var products = await mediator.Send(query, cancellationToken);
        return Results.Ok(products);
    }

    private static async Task<IResult> GetById(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        var product = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return Results.Ok(product);
    }

    private static async Task<IResult> Create(CreateProductCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        var product = await mediator.Send(command, cancellationToken);
        return Results.CreatedAtRoute("GetProductById", new { id = product.Id }, product);
    }

    private static async Task<IResult> Update(Guid id, UpdateProductCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return Results.BadRequest("Id in the URL does not match Id in the request body.");
        }
        var cmd = command with { Id = id };
        var product = await mediator.Send(cmd, cancellationToken);
        return Results.Ok(product);
    }

    private static async Task<IResult> Delete(Guid id, IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return Results.NoContent();
    }


}