using Claims.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistence;

public class ClaimRepository : IClaimRepository
{
    private readonly ClaimsContext _claimsContext;

    public ClaimRepository(ClaimsContext claimsContext)
    {
        _claimsContext = claimsContext;
    }

    public async Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _claimsContext.Claims.ToListAsync(cancellationToken);
    }

    public async Task<Claim?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _claimsContext.Claims
            .SingleOrDefaultAsync(claim => claim.Id == id, cancellationToken);
    }

    public async Task CreateAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        _claimsContext.Claims.Add(claim);
        await _claimsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var claim = await GetByIdAsync(id, cancellationToken);
        if (claim is null)
        {
            return false;
        }

        _claimsContext.Claims.Remove(claim);
        await _claimsContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}