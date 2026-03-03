using Claims.Application.Abstractions;
using Claims.Application.Common;

namespace Claims.Tests.Helper;

public sealed class FakeCoverValidator : ICoverValidator
{
    private readonly ValidationResult _result;

    public FakeCoverValidator(ValidationResult result)
    {
        _result = result;
    }

    public Task<ValidationResult> ValidateAsync(Cover cover, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_result);
    }
}