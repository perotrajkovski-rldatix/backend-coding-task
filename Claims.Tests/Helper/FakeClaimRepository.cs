using Claims.Application.Abstractions;

namespace Claims.Tests.Helper;

public sealed class FakeClaimRepository : IClaimRepository
{
    public bool CreateCalled { get; private set; }

    public Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult((IReadOnlyList<Claim>)Array.Empty<Claim>());
    }

    public Task<Claim?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Claim?>(null);
    }

    public Task CreateAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        CreateCalled = true;
        return Task.CompletedTask;
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}