using Claims.Application.Abstractions;
using Claims.Application.Validation;
using Claims.Tests.Helper;
using Xunit;

namespace Claims.Tests.Application.Validation;

public class ClaimValidatorTests
{
    [Fact]
    public async Task ValidateAsync_ShouldFail_WhenDamageCostExceedsLimit()
    {
        var repository = new FakeCoverRepository
        {
            CoverToReturn = new Cover
            {
                Id = "testcover1",
                StartDate = DateTime.UtcNow.Date.AddDays(-10),
                EndDate = DateTime.UtcNow.Date.AddDays(10),
                Type = CoverType.Yacht
            }
        };

        var sut = new ClaimValidator(repository);
        var claim = new Claim
        {
            CoverId = "testcover1",
            Created = DateTime.UtcNow.Date,
            Name = "claim",
            Type = ClaimType.Collision,
            DamageCost = 100001m
        };

        var result = await sut.ValidateAsync(claim);

        Assert.False(result.IsValid);
        Assert.Contains("DamageCost cannot exceed 100000.", result.Errors);
    }

    [Fact]
    public async Task ValidateAsync_ShouldFail_WhenRelatedCoverDoesNotExist()
    {
        var repository = new FakeCoverRepository { CoverToReturn = null };
        var sut = new ClaimValidator(repository);

        var claim = new Claim
        {
            CoverId = "missingcover",
            Created = DateTime.UtcNow.Date,
            Name = "claim",
            Type = ClaimType.Collision,
            DamageCost = 100m
        };

        var result = await sut.ValidateAsync(claim);

        Assert.False(result.IsValid);
        Assert.Contains("Related cover does not exist.", result.Errors);
    }

    [Fact]
    public async Task ValidateAsync_ShouldFail_WhenCreatedIsOutsideCoverPeriod()
    {
        var repository = new FakeCoverRepository
        {
            CoverToReturn = new Cover
            {
                Id = "testcover3",
                StartDate = DateTime.UtcNow.Date.AddDays(5),
                EndDate = DateTime.UtcNow.Date.AddDays(10),
                Type = CoverType.Yacht
            }
        };

        var sut = new ClaimValidator(repository);
        var claim = new Claim
        {
            CoverId = "testcover3",
            Created = DateTime.UtcNow.Date.AddDays(1),
            Name = "claim",
            Type = ClaimType.Collision,
            DamageCost = 100m
        };

        var result = await sut.ValidateAsync(claim);

        Assert.False(result.IsValid);
        Assert.Contains("Claim.Created must be within the period of the related Cover.", result.Errors);
    }

    [Fact]
    public async Task ValidateAsync_ShouldPass_ForValidClaim()
    {
        var repository = new FakeCoverRepository
        {
            CoverToReturn = new Cover
            {
                Id = "testcover4",
                StartDate = DateTime.UtcNow.Date.AddDays(-1),
                EndDate = DateTime.UtcNow.Date.AddDays(10),
                Type = CoverType.Yacht
            }
        };

        var sut = new ClaimValidator(repository);
        var claim = new Claim
        {
            CoverId = "testcover4",
            Created = DateTime.UtcNow.Date,
            Name = "claim",
            Type = ClaimType.Collision,
            DamageCost = 100m
        };

        var result = await sut.ValidateAsync(claim);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}
