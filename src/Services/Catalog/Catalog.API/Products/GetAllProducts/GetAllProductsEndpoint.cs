namespace Catalog.API.Products.GetAllProducts
{
    public record GetAllProductsResponse(IEnumerable<Product> Products);

    //public record GetAllProductsRequest();

    public class GetAllProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/products",
                async (ISender sender) =>
                {
                    var res = await sender.Send(new GetAllProductsQuery());

                    //var response = res.Adapt<GetAllProductsResponse>();
                    var response = new GetAllProductsResponse(res.Products.AsEnumerable());

                    return Results.Ok(response);
                })
                .WithName("GetAllProducts")
                .Produces<GetAllProductsResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Products")
                .WithDescription("Get Alls Product");
        }
    }
}
