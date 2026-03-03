using Claims.Application.Abstractions;
using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimService _claimService;
        private readonly Auditer _auditer;

        public ClaimsController(IClaimService claimService, AuditContext auditContext)
        {
            _claimService = claimService;
            _auditer = new Auditer(auditContext);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Claim>>> GetAsync(CancellationToken cancellationToken)
        {
            var claims = await _claimService.GetAllAsync(cancellationToken);
            return Ok(claims);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetAsync(string id, CancellationToken cancellationToken)
        {
            var claim = await _claimService.GetByIdAsync(id, cancellationToken);
            return claim is null ? NotFound() : Ok(claim);
        }

        [HttpPost]
        public async Task<ActionResult<Claim>> CreateAsync(Claim claim, CancellationToken cancellationToken)
        {
            var result = await _claimService.CreateAsync(claim, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }

            _auditer.AuditClaim(result.Value!.Id, "POST");
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            _auditer.AuditClaim(id, "DELETE");
            var deleted = await _claimService.DeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}
