using Catalog.API.Products;

namespace Catalog.API.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; }
        public List<string> Category { get; set; } = new();

        public static Product Create(IProductCreateValidation request)
        {
            request.ValidateAndThrow();
            return new Product
            {
                Id = Guid.NewGuid(),

                Name = request.Name,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price,
                Category = request.Category
            };
        }

        public void Update(IProductUpdateValidation request)
        {
            request.ValidateAndThrow();
            
            Name = request.Name;
            Description = request.Description;
            ImageFile = request.ImageFile;
            Price = request.Price;
            Category = request.Category;
        }
    }
}
