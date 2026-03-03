using Claims.Application.Abstractions;
using Claims.Application.Common;

namespace Claims.Tests.Helper;

public sealed class FakeClaimService : IClaimService
{
    private readonly IReadOnlyList<Claim> _claims;

    public FakeClaimService(IReadOnlyList<Claim> claims)
    {
        _claims = claims;
    }

    public Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_claims);
    }

    public Task<Claim?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_claims.FirstOrDefault(c => c.Id == id));
    }

    public Task<ServiceResult<Claim>> CreateAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ServiceResult<Claim>.Success(claim));
    }

    public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(false);
    }
}