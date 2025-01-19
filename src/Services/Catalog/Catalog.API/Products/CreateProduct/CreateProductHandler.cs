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

            return new CreateProductResult(Guid.NewGuid()); 
        }
    }
}
