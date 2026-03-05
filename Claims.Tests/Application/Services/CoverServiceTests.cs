using Claims.Application.Common;
using Claims.Application.Services;
using Claims.Tests.Helper;
using Xunit;

namespace Claims.Tests.Application.Services;

public class CoverServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldReturnInvalid_WhenValidationFails()
    {
        var repository = new FakeCoverRepository();
        var validator = new FakeCoverValidator(ValidationResult.Failure("invalid"));
        var premiumCalculator = new FakePremiumCalculator(777m);
        var sut = new CoverService(repository, validator, premiumCalculator, new FakeAuditService());

        var cover = new Cover
        {
            StartDate = DateTime.UtcNow.Date.AddDays(1),
            EndDate = DateTime.UtcNow.Date.AddDays(10),
            Type = CoverType.Yacht
        };

        var result = await sut.CreateAsync(cover);

        Assert.False(result.IsSuccess);
        Assert.Contains("invalid", result.Errors);
        Assert.False(repository.CreateCalled);
    }

    [Fact]
    public async Task CreateAsync_ShouldSetId_ComputePremium_AndPersist_WhenValidationSucceeds()
    {
        var repository = new FakeCoverRepository();
        var validator = new FakeCoverValidator(ValidationResult.Success());
        var premiumCalculator = new FakePremiumCalculator(777m);
        var sut = new CoverService(repository, validator, premiumCalculator, new FakeAuditService());

        var cover = new Cover
        {
            StartDate = DateTime.UtcNow.Date.AddDays(1),
            EndDate = DateTime.UtcNow.Date.AddDays(10),
            Type = CoverType.PassengerShip
        };

        var result = await sut.CreateAsync(cover);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.False(string.IsNullOrWhiteSpace(result.Value!.Id));
        Assert.Equal(777m, result.Value.Premium);
        Assert.True(repository.CreateCalled);
    }
}