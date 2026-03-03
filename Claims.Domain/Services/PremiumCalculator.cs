using Claims.Domain.Abstractions;

namespace Claims.Domain.Services;

public class PremiumCalculator : IPremiumCalculator
{
    public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var multiplier = coverType switch
        {
            CoverType.Yacht => 1.1m,
            CoverType.PassengerShip => 1.2m,
            CoverType.Tanker => 1.5m,
            _ => 1.3m
        };

        var premiumPerDay = 1250m * multiplier;
        var insuranceLength = (int)(endDate - startDate).TotalDays;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30)
            {
                totalPremium += premiumPerDay;
            }
            else if (i < 180)
            {
                var discount = coverType == CoverType.Yacht ? 0.05m : 0.02m;
                totalPremium += premiumPerDay * (1 - discount);
            }
            else
            {
                var discount = coverType == CoverType.Yacht ? 0.08m : 0.03m;
                totalPremium += premiumPerDay * (1 - discount);
            }
        }

        return totalPremium;
    }
}