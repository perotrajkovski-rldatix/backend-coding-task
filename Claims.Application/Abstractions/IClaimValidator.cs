using Claims.Application.Common;

namespace Claims.Application.Abstractions
{
    public interface IClaimValidator
    {
        Task<ValidationResult> ValidateAsync(Claim claim, CancellationToken cancellationToken = default);
    }
}
