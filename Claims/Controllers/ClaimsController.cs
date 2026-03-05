using Claims.Application.Abstractions;
using Claims.DTOs;
using Claims.DTOs.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimsController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ClaimResponseDto>>> GetAsync(CancellationToken cancellationToken)
        {
            var claims = await _claimService.GetAllAsync(cancellationToken);
            return Ok(claims.Select(x => x.ToResponse()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClaimResponseDto>> GetAsync(string id, CancellationToken cancellationToken)
        {
            var claim = await _claimService.GetByIdAsync(id, cancellationToken);
            return claim is null ? NotFound() : Ok(claim.ToResponse());
        }

        [HttpPost]
        public async Task<ActionResult<ClaimResponseDto>> CreateAsync(CreateClaimRequestDto request, CancellationToken cancellationToken)
        {
            var claim = request.ToDomain();
            var result = await _claimService.CreateAsync(claim, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value.ToResponse());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var deleted = await _claimService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
