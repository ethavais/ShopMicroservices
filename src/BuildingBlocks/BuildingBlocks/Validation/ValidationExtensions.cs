using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using BuildingBlocks.Validation;

public static class ValidationExtensions
    {
        /// <summary>
        /// Validates the object and throws ValidationException if invalid
        /// </summary>
        /// <typeparam name="T">Type of the object implementing IValidatable</typeparam>
        /// <param name="validatable">Object to validate</param>
        /// <exception cref="ValidationException">Thrown when validation fails</exception>
        public static void ValidateAndThrow<T>(this T validatable) where T : IValidatable
        {
            if (!validatable.Validate(out var errors))
                throw new ValidationException(errors);
        }

        public static void AddError(this List<string> errors, string errorMessage)
        {
            errors.Add(errorMessage + Environment.NewLine);
        }

        /// <summary>
        /// Validates the object and returns a Result with errors if invalid
        /// </summary>
        /// <typeparam name="T">Type of the object implementing IValidatable</typeparam>
        /// <param name="validatable">Object to validate</param>
        /// <returns>Result object with validation status</returns>
        public static Result Validate<T>(this T validatable) where T : IValidatable
        {
            return validatable.Validate(out var errors) 
                ? Result.Success() 
                : Result.Failure(errors);
        }
        
        private static string GetMemberName(string? expression)
        {
            // Extract the member name from the expression string
            return expression?.Split('.', StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? "Field";
        }

        /// <summary>
        /// Validates that a string is not null or empty
        /// </summary>
        /// <param name="value">The string value to validate</param>
        /// <param name="errors">List of validation errors</param>
        /// <param name="fieldExpression">Expression representing the field being validated</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateNotNullOrEmpty(
            this string value, 
            List<string> errors,
            [CallerArgumentExpression("value")] string? fieldExpression = null)
        {
            var fieldName = GetMemberName(fieldExpression);
            if (string.IsNullOrEmpty(value))
            {
                errors.AddError($"[{fieldName}] is required.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a string meets the minimum length requirement
        /// </summary>
        /// <param name="value">The string value to validate</param>
        /// <param name="minLength">Minimum allowed length</param>
        /// <param name="errors">List of validation errors</param>
        /// <param name="fieldExpression">Expression representing the field being validated</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateMinLength(
            this string value, 
            int minLength,
            List<string> errors,
            [CallerArgumentExpression("value")] string? fieldExpression = null)
        {
            var fieldName = GetMemberName(fieldExpression);
            if (string.IsNullOrEmpty(value) || value.Length < minLength)
            {
                errors.AddError($"[{fieldName}] must be at least {minLength} characters long.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a string does not exceed the maximum length
        /// </summary>
        /// <param name="value">The string value to validate</param>
        /// <param name="maxLength">Maximum allowed length</param>
        /// <param name="errors">List of validation errors</param>
        /// <param name="fieldExpression">Expression representing the field being validated</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateMaxLength(
            this string value, 
            int maxLength,
            List<string> errors,
            [CallerArgumentExpression("value")] string? fieldExpression = null)
        {
            var fieldName = GetMemberName(fieldExpression);
            if (value != null && value.Length > maxLength)
            {
                errors.AddError($"[{fieldName}] cannot exceed {maxLength} characters.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a numeric value is greater than a minimum value
        /// </summary>
        /// <param name="value">The numeric value to validate</param>
        /// <param name="min">Minimum allowed value</param>
        /// <param name="errors">List of validation errors</param>
        /// <param name="fieldExpression">Expression representing the field being validated</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateMinValue(
            this decimal value, 
            decimal min,
            List<string> errors,
            [CallerArgumentExpression("value")] string? fieldExpression = null)
        {
            var fieldName = GetMemberName(fieldExpression);
            if (value < min)
            {
                errors.AddError($"[{fieldName}] must be greater than or equal to {min}.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a numeric value does not exceed a maximum value
        /// </summary>
        /// <param name="value">The numeric value to validate</param>
        /// <param name="max">Maximum allowed value</param>
        /// <param name="errors">List of validation errors</param>
        /// <param name="fieldExpression">Expression representing the field being validated</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateMaxValue(
            this decimal value, 
            decimal max,
            List<string> errors,
            [CallerArgumentExpression("value")] string? fieldExpression = null)
        {
            var fieldName = GetMemberName(fieldExpression);
            if (value > max)
            {
                errors.AddError($"[{fieldName}] must be less than or equal to {max}.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a string matches a regular expression pattern
        /// </summary>
        /// <param name="value">The string value to validate</param>
        /// <param name="pattern">Regular expression pattern</param>
        /// <param name="errors">List of validation errors</param>
        /// <param name="fieldExpression">Expression representing the field being validated</param>
        /// <param name="errorMessage">Custom error message</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidatePattern(
            this string value, 
            string pattern,
            List<string> errors,
            [CallerArgumentExpression("value")] string? fieldExpression = null,
            string? errorMessage = null)
        {
            var fieldName = GetMemberName(fieldExpression);
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, pattern))
            {
                errors.AddError(errorMessage ?? $"[{fieldName}] has an invalid format.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that a list is not null or empty
        /// </summary>
        /// <typeparam name="T">Type of the list elements</typeparam>
        /// <param name="list">The list to validate</param>
        /// <param name="errors">List of validation errors</param>
        /// <param name="fieldExpression">Expression representing the field being validated</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool ValidateNotEmpty<T>(
            this IEnumerable<T> list, 
            List<string> errors,
            [CallerArgumentExpression("list")] string? fieldExpression = null)
        {
            var fieldName = GetMemberName(fieldExpression);
            if (list == null || !list.Any())
            {
                errors.AddError($"[{fieldName}] must contain at least one item.");
                return false;
            }
            return true;
        }

    }
