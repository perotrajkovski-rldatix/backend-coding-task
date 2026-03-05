using Claims.Application.Common;

namespace Claims.Application.Abstractions
{
    public interface ICoverValidator
    {
        // Intentionally asynchronous to keep a consistent validator contract.
        // This allows future I/O-backed checks (for example repository lookups)
        // without changing callers.
        Task<ValidationResult> ValidateAsync(Cover cover, CancellationToken cancellationToken = default);
    }
}
