namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductResult(bool IsSuccess);

    public record UpdateProductCommand(
        Guid Id,
        uint Version,
        string Name,
        string Description,
        string ImageFile,
        decimal Price,
        List<string> Category
    ) : IProductUpdateValidation, ICommand<UpdateProductResult>
    {
        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();
            return this.ValidateForUpdate(errors);
        }
    }

    public class UpdateProductCommandHandler(IDocumentSession session) 
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(
            UpdateProductCommand command, 
            CancellationToken cancellationToken)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            try
            {
                var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
                
                if (product is null)
                    throw new Exception($"Product not found with ID: {command.Id}");
                
                product.Update(command);
                session.Store(product);
                await session.SaveChangesAsync(cancellationToken);
                
                return new UpdateProductResult(true);
            }
            catch (Marten.Exceptions.ConcurrencyException ex)
            {
                Console.WriteLine($"Concurrency conflict detected for product {command.Id}: {ex.Message}");
                return new UpdateProductResult(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product {command.Id}: {ex.Message}");
                return new UpdateProductResult(false);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Update product {command.Id} took {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
} 