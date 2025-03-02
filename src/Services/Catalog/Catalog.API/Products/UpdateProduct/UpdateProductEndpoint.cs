namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductResponse(Guid Id);
    
    public record UpdateProductRequest(
        Guid Id,
        uint Version,
        string Name,
        string Description,
        string ImageFile,
        decimal Price,
        List<string> Category
    ) : IProductUpdateValidation
    {
        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();
            return this.ValidateForUpdate(errors);
        }
    }
    
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", 
                async (UpdateProductRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateProductCommand>();
                    var result = await sender.Send(command);
                    return Results.Ok(result.Adapt<UpdateProductResponse>());
                })
                .WithName("UpdateProduct")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
} 