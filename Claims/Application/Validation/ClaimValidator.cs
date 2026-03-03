using Claims.Application.Abstractions;
using Claims.Application.Common;

namespace Claims.Application.Validation;

public class ClaimValidator : IClaimValidator
{
    private const decimal MaxDamageCost = 100000m;
    private readonly ICoverRepository _coverRepository;

    public ClaimValidator(ICoverRepository coverRepository)
    {
        _coverRepository = coverRepository;
    }

    public async Task<ValidationResult> ValidateAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        if (claim.DamageCost > MaxDamageCost)
        {
            errors.Add("DamageCost cannot exceed 100000.");
        }

        var cover = await _coverRepository.GetByIdAsync(claim.CoverId, cancellationToken);
        if (cover is null)
        {
            errors.Add("Related cover does not exist.");
            return ValidationResult.Failure(errors);
        }

        var createdDate = claim.Created.Date;
        if (createdDate < cover.StartDate.Date || createdDate > cover.EndDate.Date)
        {
            errors.Add("Claim.Created must be within the period of the related Cover.");
        }

        return errors.Count == 0 ? ValidationResult.Success() : ValidationResult.Failure(errors);
    }
}
