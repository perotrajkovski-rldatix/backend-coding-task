using Claims.Application.Common;
using Claims.Application.Services;
using Claims.Tests.Helper;
using Xunit;

namespace Claims.Tests.Application.Services;

public class ClaimServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldReturnInvalid_WhenValidationFails()
    {
        var repository = new FakeClaimRepository();
        var validator = new FakeClaimValidator(ValidationResult.Failure("invalid"));
        var sut = new ClaimService(repository, validator);

        var claim = new Claim
        {
            CoverId = "testcover1",
            Created = DateTime.UtcNow.Date,
            Name = "testcover1",
            Type = ClaimType.Collision,
            DamageCost = 100m
        };

        var result = await sut.CreateAsync(claim);

        Assert.False(result.IsSuccess);
        Assert.Contains("invalid", result.Errors);
        Assert.False(repository.CreateCalled);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetId_AndPersist_WhenValidationSucceeds()
    {
        var repository = new FakeClaimRepository();
        var validator = new FakeClaimValidator(ValidationResult.Success());
        var sut = new ClaimService(repository, validator);

        var claim = new Claim
        {
            CoverId = "testcover2",
            Created = DateTime.UtcNow.Date,
            Name = "testcover2",
            Type = ClaimType.Collision,
            DamageCost = 100m
        };

        var result = await sut.CreateAsync(claim);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.False(string.IsNullOrWhiteSpace(result.Value!.Id));
        Assert.True(repository.CreateCalled);
    }
}
