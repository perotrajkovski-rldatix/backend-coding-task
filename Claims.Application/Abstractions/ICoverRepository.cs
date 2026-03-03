namespace Claims.Application.Abstractions;

public interface ICoverRepository
{
    Task<IReadOnlyList<Cover>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Cover?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task CreateAsync(Cover cover, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}