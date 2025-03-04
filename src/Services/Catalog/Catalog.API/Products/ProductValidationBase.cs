using System.Collections.Generic;

namespace Catalog.API.Products
{
    public interface IProductBaseValidation : IValidatable
    {
        string Name { get; }
        string Description { get; }
        string ImageFile { get; }
        decimal Price { get; }
        List<string> Category { get; }
    }

    public static partial class ProductValidationExtensions
    {
        public static void ValidateBaseProduct(
            this IProductBaseValidation model,
            List<string> errors)
        {
            model.Name.ValidateNotNullOrEmpty(errors);
            model.Name.ValidateMinLength(3, errors);
            model.Name.ValidateMaxLength(100, errors);

            model.Description.ValidateNotNullOrEmpty(errors);
            model.Description.ValidateMaxLength(500, errors);

            model.Price.ValidateMinValue(0.01m, errors);
            model.Price.ValidateMaxValue(10000m, errors);

            model.Category.ValidateNotEmpty(errors);
        }
    }
}
