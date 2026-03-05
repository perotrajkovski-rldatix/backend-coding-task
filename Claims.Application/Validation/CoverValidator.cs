using Claims.Application.Abstractions;
using Claims.Application.Common;

namespace Claims.Application.Validation;

public class CoverValidator : ICoverValidator
{
    // Intentionally returns Task to keep parity with validator abstractions and
    // support future async validation checks without API changes.
    public Task<ValidationResult> ValidateAsync(Cover cover, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();
        var today = DateTime.UtcNow.Date;

        if (cover.StartDate.Date < today)
        {
            errors.Add("Cover.StartDate cannot be in the past.");
        }

        if (cover.EndDate.Date <= cover.StartDate.Date)
        {
            errors.Add("Cover.EndDate must be after StartDate.");
        }

        var totalDays = (cover.EndDate.Date - cover.StartDate.Date).TotalDays;
        if (totalDays > 365)
        {
            errors.Add("Total insurance period cannot exceed 1 year.");
        }

        var result = errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors);

        return Task.FromResult(result);
    }
}