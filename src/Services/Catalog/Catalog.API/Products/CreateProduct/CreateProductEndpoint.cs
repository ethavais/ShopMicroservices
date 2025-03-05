namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductResponse(Guid Id);

    public record CreateProductRequest(
        string Name,
        string Description,
        string ImageFile,
        decimal Price,
        List<string> Category
    ) : IProductCreateValidation
    {
        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();
            return this.ValidateForCreate(errors);
        }
    }

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/products",
                async (CreateProductRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateProductCommand>();

                    var res = await sender.Send(command);

                    var response = res.Adapt<CreateProductResponse>();

                    return Results.Created($"/products/{response.Id}", response);
                })
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
