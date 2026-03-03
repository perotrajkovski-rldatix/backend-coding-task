namespace Claims.Application.Common
{
    public sealed class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public bool IsNotFound { get; set; }
        public T? Value { get; set; }
        public IReadOnlyList<string> Errors { get; set; }
        public ServiceResult(bool isSuccess, bool isNotFound, T? value, IReadOnlyList<string> errors)
        {
            IsSuccess = isSuccess;
            IsNotFound = isNotFound;
            Value = value;
            Errors = errors;
        }
        public static ServiceResult<T> Success(T value) => new ServiceResult<T>(true, false, value, Array.Empty<string>());
        public static ServiceResult<T> Invalid(IEnumerable<string> errors) => new ServiceResult<T>(false, false, default, errors.ToArray());
        public static ServiceResult<T> NotFound() => new ServiceResult<T>(false, true, default, Array.Empty<string>());
    }
}
