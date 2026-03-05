    using Claims.Controllers;
using Claims.DTOs.Claims;
using Claims.Tests.Helper;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Claims.Tests
{
    public class ClaimsControllerTests
    {
        [Fact]
        public async Task Get_Claims()
        {
            var claim = new Claim
            {
                Id = "1",
                CoverId = "test",
                Created = DateTime.UtcNow,
                Name = "Testing",
                Type = ClaimType.Collision,
                DamageCost = 1200m
            };

            var controller = new ClaimsController(new FakeClaimService([claim]));

            var result = await controller.GetAsync(CancellationToken.None);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsAssignableFrom<List<ClaimResponseDto>>(okResult.Value);

            var item = Assert.Single(payload);
            Assert.Equal(claim.Id, item.Id);
            Assert.Equal(claim.CoverId, item.CoverId);
            Assert.Equal(claim.Name, item.Name);
            Assert.Equal(claim.Type, item.Type);
            Assert.Equal(claim.DamageCost, item.DamageCost);
        }
    }
}
