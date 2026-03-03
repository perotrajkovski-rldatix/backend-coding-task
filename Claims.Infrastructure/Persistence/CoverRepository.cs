using Claims.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistence;

public class CoverRepository : ICoverRepository
{
    private readonly ClaimsContext _claimsContext;

    public CoverRepository(ClaimsContext claimsContext)
    {
        _claimsContext = claimsContext;
    }

    public async Task<IReadOnlyList<Cover>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _claimsContext.Covers.ToListAsync(cancellationToken);
    }

    public async Task<Cover?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _claimsContext.Covers
            .SingleOrDefaultAsync(cover => cover.Id == id, cancellationToken);
    }

    public async Task CreateAsync(Cover cover, CancellationToken cancellationToken = default)
    {
        _claimsContext.Covers.Add(cover);
        await _claimsContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var cover = await GetByIdAsync(id, cancellationToken);
        if (cover is null)
        {
            return false;
        }

        _claimsContext.Covers.Remove(cover);
        await _claimsContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}