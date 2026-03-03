namespace Claims.Application.Abstractions
{
    public interface IPremiumCalculator
    {
        decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
    }
}
