namespace Catalog.API.Products.DeleteProductById
{
    public record DeleteProductByIdResponse(bool IsSuccess);

    public class DeleteProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete(
                "/products/{id}",
                async (Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new DeleteProductByIdCommand(id));
                    var response = new DeleteProductByIdResponse(result.IsSuccess);

                    return result.IsSuccess 
                        ? Results.Ok(response) 
                        : Results.NotFound(response);
                })
                .WithName("DeleteProductById")
                .Produces<DeleteProductByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Product by ID")
                .WithDescription("Delete a product by its ID");
        }
    }
}
