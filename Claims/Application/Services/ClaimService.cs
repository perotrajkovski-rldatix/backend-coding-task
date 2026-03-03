using Claims.Application.Abstractions;
using Claims.Application.Common;

namespace Claims.Application.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IClaimValidator _claimValidator;
        public ClaimService(IClaimRepository claimRepository, IClaimValidator claimValidator)
        {
            _claimRepository = claimRepository;
            _claimValidator = claimValidator;
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

            return ServiceResult<Claim>.Success(claim);
        }
        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _claimRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
