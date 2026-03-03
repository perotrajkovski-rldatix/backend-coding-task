using Claims.Application.Abstractions;
using Claims.Application.Common;

namespace Claims.Application.Services;

public class CoverService : ICoverService
{
    private readonly ICoverRepository _coverRepository;
    private readonly ICoverValidator _coverValidator;
    private readonly IPremiumCalculator _premiumCalculator;

    public CoverService(
        ICoverRepository coverRepository,
        ICoverValidator coverValidator,
        IPremiumCalculator premiumCalculator)
    {
        _coverRepository = coverRepository;
        _coverValidator = coverValidator;
        _premiumCalculator = premiumCalculator;
    }

    public async Task<IReadOnlyList<Cover>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _coverRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Cover?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _coverRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<ServiceResult<Cover>> CreateAsync(Cover cover, CancellationToken cancellationToken = default)
    {
        var validation = await _coverValidator.ValidateAsync(cover, cancellationToken);
        if (!validation.IsValid)
        {
            return ServiceResult<Cover>.Invalid(validation.Errors);
        }

        cover.Id = Guid.NewGuid().ToString();
        cover.Premium = _premiumCalculator.ComputePremium(cover.StartDate, cover.EndDate, cover.Type);

        await _coverRepository.CreateAsync(cover, cancellationToken);
        return ServiceResult<Cover>.Success(cover);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _coverRepository.DeleteAsync(id, cancellationToken);
    }

    public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        return _premiumCalculator.ComputePremium(startDate, endDate, coverType);
    }
}
