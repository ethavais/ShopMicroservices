namespace Catalog.API.Products.DeleteProductById
{
    public record DeleteProductByIdResult(bool IsSuccess);

    public record DeleteProductByIdCommand(Guid Id) : ICommand<DeleteProductByIdResult>;

    public class DeleteProductByIdHandler(IDocumentSession session, IInjectableLogger logger)
        : ICommandHandler<DeleteProductByIdCommand, DeleteProductByIdResult>
    {
        public async Task<DeleteProductByIdResult> Handle(
            DeleteProductByIdCommand command,
            CancellationToken cancellationToken)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                logger.Inform($"Starting to delete product with ID: {command.Id}");

                var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

                if (product is null)
                {
                    logger.Error($"Product not found with ID: {command.Id}", new Exception("Product not found"));
                    return new DeleteProductByIdResult(false);
                }

                session.Delete(product);
                await session.SaveChangesAsync(cancellationToken);

                logger.Inform($"Successfully deleted product with ID: {command.Id}");
                return new DeleteProductByIdResult(true);
            }
            catch (Exception ex)
            {
                logger.Error($"Error deleting product with ID: {command.Id}", ex);
                return new DeleteProductByIdResult(false);
            }
            finally
            {
                stopwatch.Stop();
                logger.Inform($"Delete product {command.Id} took {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
}
