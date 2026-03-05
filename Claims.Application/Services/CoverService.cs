using Claims.Application.Abstractions;
using Claims.Application.Abstractions.Auditing;
using Claims.Application.Common;
using Claims.Domain.Abstractions;

namespace Claims.Application.Services;

public class CoverService : ICoverService
{
    private readonly ICoverRepository _coverRepository;
    private readonly ICoverValidator _coverValidator;
    private readonly IPremiumCalculator _premiumCalculator;
    private readonly IAuditService _auditService;

    public CoverService(
        ICoverRepository coverRepository,
        ICoverValidator coverValidator,
        IPremiumCalculator premiumCalculator,
        IAuditService auditService)
    {
        _coverRepository = coverRepository;
        _coverValidator = coverValidator;
        _premiumCalculator = premiumCalculator;
        _auditService = auditService;
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
        await _auditService.AuditCoverAsync(cover.Id, "POST");
        return ServiceResult<Cover>.Success(cover);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var deleted = await _coverRepository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            await _auditService.AuditCoverAsync(id, "DELETE");
        }

        return deleted;
    }

    public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        return _premiumCalculator.ComputePremium(startDate, endDate, coverType);
    }
}
