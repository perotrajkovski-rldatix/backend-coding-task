namespace Claims.Application.Common
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public IReadOnlyList<string> Errors{ get; set; }

        private ValidationResult(bool isValid, IReadOnlyList<string> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }
        public static ValidationResult Success()
        {
            return new ValidationResult(true, Array.Empty<string>());
        }
        public static ValidationResult Failure(params string[] errors)
        {
            return new ValidationResult(false, errors);
        }
        public static ValidationResult Failure(IEnumerable<string> errors)
        {
            return new ValidationResult(false, errors.ToArray());
        }
    }
}
