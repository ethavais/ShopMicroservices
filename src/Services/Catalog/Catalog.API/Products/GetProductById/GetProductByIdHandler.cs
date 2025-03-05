namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResult(Product Product);

    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

    internal class GetProductByIdHandler(
        IDocumentSession session, 
        IInjectableLogger logger) : 
        IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(
            GetProductByIdQuery query,
            CancellationToken cancellationToken)
        {
            var id = query.Id;  
            logger.Inform($"Starting query product(Id: {id}): " + query);

            var product = await session.LoadAsync<Product>(id, cancellationToken);

            if (product is null)
            {
                logger.Error(
                    $"Product not found with ID: {id}", 
                    new Exception($"Product not found with ID: {id}"));
                throw new ArgumentNullException(nameof(product), "Product not found.");
            }

            return new GetProductByIdResult(product);
        }
    }
}
