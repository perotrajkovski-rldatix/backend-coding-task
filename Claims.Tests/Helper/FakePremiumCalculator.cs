using Claims.Domain.Abstractions;

namespace Claims.Tests.Helper;

public sealed class FakePremiumCalculator : IPremiumCalculator
{
    private readonly decimal _premium;

    public FakePremiumCalculator(decimal premium)
    {
        _premium = premium;
    }

    public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        return _premium;
    }
}