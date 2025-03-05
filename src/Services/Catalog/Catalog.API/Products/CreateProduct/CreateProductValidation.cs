namespace Catalog.API.Products.CreateProduct
{
    public interface IProductCreateValidation : IProductBaseValidation
    { }

    public static partial class ProductValidationExtensions
    {
        public static bool ValidateForCreate(
            this IProductCreateValidation model,
            List<string> errors)
        {
            model.ValidateBaseProduct(errors);

            model.ImageFile.ValidateNotNullOrEmpty(errors);
            model.ImageFile.ValidatePattern(@"\.(jpg|png|gif)$", errors,
                "Image file must be a valid image (jpg, png, gif)");

            return errors.Count == 0;
        }
    }
}

