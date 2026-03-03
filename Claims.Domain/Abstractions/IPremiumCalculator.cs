namespace Claims.Domain.Abstractions;

public interface IPremiumCalculator
{
    decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
}
