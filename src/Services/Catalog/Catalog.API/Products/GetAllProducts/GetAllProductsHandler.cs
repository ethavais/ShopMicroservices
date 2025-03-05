namespace Catalog.API.Products.GetAllProducts
{
    public record GetAllProductsResult(IEnumerable<Product> Products);

    public record GetAllProductsQuery() : IQuery<GetAllProductsResult>;

    internal class GetAllProductsQueryHandler(
        IDocumentSession session,
        IInjectableLogger logger) : 
        IQueryHandler<GetAllProductsQuery, GetAllProductsResult>
    {
        public async Task<GetAllProductsResult> Handle(
            GetAllProductsQuery query, 
            CancellationToken cancellationToken)
        {
            logger.Inform("Starting query all products: " + query);
            
            var products = await session.Query<Product>().ToListAsync(cancellationToken);
            
            return new GetAllProductsResult(products);
        }
    }
}
