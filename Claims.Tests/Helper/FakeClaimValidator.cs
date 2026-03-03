using Claims.Application.Abstractions;
using Claims.Application.Common;

namespace Claims.Tests.Helper;

public sealed class FakeClaimValidator : IClaimValidator
{
    private readonly ValidationResult _result;

    public FakeClaimValidator(ValidationResult result)
    {
        _result = result;
    }

    public Task<ValidationResult> ValidateAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_result);
    }
}