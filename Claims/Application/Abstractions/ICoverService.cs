using Claims.Application.Common;

namespace Claims.Application.Abstractions
{
    public interface ICoverService
    {
        Task<IReadOnlyList<Cover>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Cover?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<ServiceResult<Cover>> CreateAsync(Cover cover, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
        decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
    }
}
