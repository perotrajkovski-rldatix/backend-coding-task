using Claims.Application.Abstractions;
using Claims.Application.Abstractions.Auditing;
using Claims.Application.Common;

namespace Claims.Application.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IClaimValidator _claimValidator;
        private readonly IAuditService _auditService;

        public ClaimService(
            IClaimRepository claimRepository,
            IClaimValidator claimValidator,
            IAuditService auditService)
        {
            _claimRepository = claimRepository;
            _claimValidator = claimValidator;
            _auditService = auditService;
        }
        public async Task<IReadOnlyList<Claim>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _claimRepository.GetAllAsync(cancellationToken);
        }
        public async Task<Claim?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _claimRepository.GetByIdAsync(id, cancellationToken);
        }
        public async Task<ServiceResult<Claim>> CreateAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            var validation = await _claimValidator.ValidateAsync(claim, cancellationToken);
            if (!validation.IsValid)
            {
                return ServiceResult<Claim>.Invalid(validation.Errors);
            }
            claim.Id = Guid.NewGuid().ToString();
            await _claimRepository.CreateAsync(claim, cancellationToken);
            await _auditService.AuditClaimAsync(claim.Id, "POST");

            return ServiceResult<Claim>.Success(claim);
        }
        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var deleted = await _claimRepository.DeleteAsync(id, cancellationToken);
            if (deleted)
            {
                await _auditService.AuditClaimAsync(id, "DELETE");
            }

            return deleted;
        }
    }
}
