using Claims.Application.Abstractions;
using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ILogger<ClaimsController> _logger;
        private readonly IClaimRepository _claimRepository;
        private readonly Auditer _auditer;

        public ClaimsController(
            ILogger<ClaimsController> logger,
            IClaimRepository claimRepository,
            AuditContext auditContext)
        {
            _logger = logger;
            _claimRepository = claimRepository;
            _auditer = new Auditer(auditContext);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Claim>>> GetAsync(CancellationToken cancellationToken)
        {
            var claims = await _claimRepository.GetAllAsync(cancellationToken);
            return Ok(claims);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Claim>> GetAsync(string id, CancellationToken cancellationToken)
        {
            var claim = await _claimRepository.GetByIdAsync(id, cancellationToken);
            return claim is null ? NotFound() : Ok(claim);
        }

        [HttpPost]
        public async Task<ActionResult<Claim>> CreateAsync(Claim claim, CancellationToken cancellationToken)
        {
            claim.Id = Guid.NewGuid().ToString();
            await _claimRepository.CreateAsync(claim, cancellationToken);
            _auditer.AuditClaim(claim.Id, "POST");
            return Ok(claim);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            _auditer.AuditClaim(id, "DELETE");
            var deleted = await _claimRepository.DeleteAsync(id, cancellationToken);
            return deleted ? NoContent() : NotFound();
        }
    }
}
