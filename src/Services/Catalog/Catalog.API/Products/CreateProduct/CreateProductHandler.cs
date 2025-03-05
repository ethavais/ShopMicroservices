namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductResult(Guid Id);

    public record CreateProductCommand(
        string Name,
        string Description,
        string ImageFile,
        decimal Price,
        List<string> Category
    ) : IProductCreateValidation, ICommand<CreateProductResult>
    {
        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();
            return this.ValidateForCreate(errors);
        }
    }

    public class CreateProductCommandHandler(IDocumentSession session) : 
        ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(
            CreateProductCommand command, 
            CancellationToken cancellationToken)
        {
            var product = Product.Create(command);
            
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id); 
        }
    }
}


            #region Console check
            // Console.WriteLine("\n\n\nCreated Product: ");
            // Console.WriteLine($"Name: {product.Name}");
            // Console.WriteLine($"Description: {product.Description}");
            // Console.WriteLine($"Image File: {product.ImageFile}");
            // Console.WriteLine($"Price: {product.Price}");
            // Console.WriteLine($"Category: {string.Join(", ", product.Category)}");
            #endregion