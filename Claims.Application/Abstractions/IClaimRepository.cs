namespace Claims.Application.Abstractions;

public interface IClaimRepository
{
    Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Claim?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task CreateAsync(Claim claim, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}