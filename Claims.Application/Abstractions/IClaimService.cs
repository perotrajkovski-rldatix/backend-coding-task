using Claims.Application.Common;

namespace Claims.Application.Abstractions;

public interface IClaimService
{
    Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Claim?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<ServiceResult<Claim>> CreateAsync(Claim claim, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
