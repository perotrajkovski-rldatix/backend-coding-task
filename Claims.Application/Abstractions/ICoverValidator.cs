using Claims.Application.Common;

namespace Claims.Application.Abstractions
{
    public interface ICoverValidator
    {
        Task<ValidationResult> ValidateAsync(Cover cover, CancellationToken cancellationToken = default);
    }
}
