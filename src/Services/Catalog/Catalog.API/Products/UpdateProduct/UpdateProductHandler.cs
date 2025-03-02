namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductResult(Guid Id);

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
                // Using a more efficient direct query with version check
                var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
                
                if (product is null)
                    throw new Exception($"Product not found with ID: {command.Id}");
                
                // Apply the updates
                product.Update(command);
                
                // Store the updated product
                session.Store(product);
                
                // Commit changes in one transaction
                await session.SaveChangesAsync(cancellationToken);
                
                return new UpdateProductResult(command.Id);
            }
            catch (Marten.Exceptions.ConcurrencyException ex)
            {
                // Log the concurrency conflict
                Console.WriteLine($"Concurrency conflict detected for product {command.Id}: {ex.Message}");
                throw new Exception($"The product was modified by another user. Please refresh and try again.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product {command.Id}: {ex.Message}");
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Update product {command.Id} took {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
} 