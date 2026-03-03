using Claims.Application.Validation;
using System.Runtime.InteropServices;
using Xunit;

namespace Claims.Tests.Application.Validation
{
    public class CoverValidatorTests
    {
        private readonly CoverValidator _validator = new();
        [Fact]
        public async Task ValidateAsync_ShouldFail_WhenStartDateIsInPast()
        {
            var cover = new Cover
            {
                StartDate = DateTime.UtcNow.Date.AddDays(-1),
                EndDate = DateTime.UtcNow.Date.AddDays(10),
                Type = CoverType.Yacht
            };

            var result = await _validator.ValidateAsync(cover);

            Assert.False(result.IsValid);
            Assert.Contains("Cover.StartDate cannot be in the past.", result.Errors);
        }
        [Fact]
        public async Task ValidateAsync_ShouldPass_ForValidCover()
        {
            var start = DateTime.UtcNow.Date.AddDays(1);
            var cover = new Cover
            {
                StartDate = start,
                EndDate = DateTime.UtcNow.AddDays(30),
                Type = CoverType.Yacht
            };

            var result = await _validator.ValidateAsync(cover);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
