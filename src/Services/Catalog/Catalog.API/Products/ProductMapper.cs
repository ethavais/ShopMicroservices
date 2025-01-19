using Catalog.API.Models;

namespace Catalog.API.Products
{
    public interface IProductMappable
    {
        Product MapToProduct();
    }

    public static class ProductMapperExtensions
    {
        public static Product MapToProduct(this IProductMappable command)
            => command.MapToProduct();
    }

}
