using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductResult(Guid Id);

    public record CreateProductCommand(
        string Name,
        string Description,
        string ImageFile,
        decimal Price,
        List<string> Category
    ) : ICommand<CreateProductResult>, IProductMappable
    {
        public Product MapToProduct()
            => new Product
            {
                Name = Name,
                Description = Description,
                ImageFile = ImageFile,
                Price = Price,
                Category = Category
            };
    }

    public class CreateProductCommandHandler : 
        ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(
            CreateProductCommand command, 
            CancellationToken cancellationToken)
        {
            var createProduct = command.MapToProduct();

            // Log the created product to the console
            Console.WriteLine("\n\n\nCreated Product: ");
            Console.WriteLine($"Name: {createProduct.Name}");
            Console.WriteLine($"Description: {createProduct.Description}");
            Console.WriteLine($"Image File: {createProduct.ImageFile}");
            Console.WriteLine($"Price: {createProduct.Price}");
            Console.WriteLine($"Category: {string.Join(", ", createProduct.Category)}");

            return new CreateProductResult(Guid.NewGuid()); 
        }
    }
}
