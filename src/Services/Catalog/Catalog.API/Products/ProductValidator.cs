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

    public interface IProductCreateValidation : IProductBaseValidation 
    { }
    public interface IProductUpdateValidation : IProductBaseValidation 
    {
        Guid Id { get; }
        uint Version { get; }
    }

    public static class ProductValidationExtensions
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

        public static bool ValidateForUpdate(
            this IProductUpdateValidation model, 
            List<string> errors)
        {
            model.ValidateBaseProduct(errors);
            
            if(model.Id == Guid.Empty)
                errors.Add("Product ID is required");
                
            if(model.Version < 1)
                errors.Add("Invalid version");

            return errors.Count == 0;
        }
    }
}