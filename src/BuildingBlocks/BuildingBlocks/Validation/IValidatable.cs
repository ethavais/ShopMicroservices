namespace BuildingBlocks.Validation
{
    public interface IValidatable
    {
        bool Validate(out List<string> errors);
    }

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors)
            : base($"\n[!!!] Validation Failed [!!!] \n\t{string.Join("\t", errors)}")
        {
            Errors = errors;
        }
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public List<string> Errors { get; }
        public bool IsFailure => !IsSuccess;

        private Result(bool isSuccess, List<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => new Result(true, new List<string>());
        public static Result Failure(List<string> errors) => new Result(false, errors);
    }
}