namespace Catalog.API.Products
{
    public interface IProductUpdateValidation : IProductBaseValidation
    {
        //Guid Id { get; }
        //uint Version { get; }
    }

    public static partial class ProductValidationExtensions
    {
        public static bool ValidateForUpdate(
            this IProductUpdateValidation model,
            List<string> errors)
        {
            model.ValidateBaseProduct(errors);

            model.ImageFile.ValidatePattern(@"\.(jpg|png|gif)$", errors,
    "Image file must be a valid image (jpg, png, gif)");
            //if (model.Id == Guid.Empty)
            //    errors.Add("Product ID is required");

            //if (model.Version < 1)
            //    errors.Add("Invalid version");

            return errors.Count == 0;
        }
    }
}
