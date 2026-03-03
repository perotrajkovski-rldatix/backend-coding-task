using Claims.Domain.Services;
using Xunit;

namespace Claims.Tests.Domain.Services;

public class PremiumCalculatorTests
{
    private readonly PremiumCalculator _sut = new();

    [Fact]
    public void ComputePremium_ShouldReturnZero_WhenPeriodIsZeroDays()
    {
        var start = new DateTime(2026, 1, 1);
        var end = start;

        var premium = _sut.ComputePremium(start, end, CoverType.Yacht);

        Assert.Equal(0m, premium);
    }

    [Fact]
    public void ComputePremium_ShouldUseBaseTier_ForFirst30Days()
    {
        var start = new DateTime(2026, 1, 1);
        var end = start.AddDays(10);

        var premium = _sut.ComputePremium(start, end, CoverType.Yacht);

        var expected = ExpectedPremium(start, end, CoverType.Yacht);
        Assert.Equal(expected, premium);
    }

    [Fact]
    public void ComputePremium_ShouldUseSecondTierDiscount_AfterDay30()
    {
        var start = new DateTime(2026, 1, 1);
        var end = start.AddDays(40);

        var premium = _sut.ComputePremium(start, end, CoverType.PassengerShip);

        var expected = ExpectedPremium(start, end, CoverType.PassengerShip);
        Assert.Equal(expected, premium);
    }

    [Fact]
    public void ComputePremium_ShouldUseThirdTierAdditionalDiscount_AfterDay180()
    {
        var start = new DateTime(2026, 1, 1);
        var end = start.AddDays(200);

        var premium = _sut.ComputePremium(start, end, CoverType.Tanker);

        var expected = ExpectedPremium(start, end, CoverType.Tanker);
        Assert.Equal(expected, premium);
    }

    private static decimal ExpectedPremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var baseRate = 1250m;
        var multiplier = coverType switch
        {
            CoverType.Yacht => 1.1m,
            CoverType.PassengerShip => 1.2m,
            CoverType.Tanker => 1.5m,
            _ => 1.3m
        };

        var dailyRate = baseRate * multiplier;
        var days = (int)(endDate - startDate).TotalDays;

        var total = 0m;
        for (var day = 0; day < days; day++)
        {
            if (day < 30)
            {
                total += dailyRate;
            }
            else if (day < 180)
            {
                var discount = coverType == CoverType.Yacht ? 0.05m : 0.02m;
                total += dailyRate * (1 - discount);
            }
            else
            {
                // "additional" discount over previous tier:
                // Yacht: 5% + 3% = 8%, Others: 2% + 1% = 3%
                var discount = coverType == CoverType.Yacht ? 0.08m : 0.03m;
                total += dailyRate * (1 - discount);
            }
        }

        return total;
    }
}