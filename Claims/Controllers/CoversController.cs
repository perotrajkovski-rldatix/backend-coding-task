using Claims.Application.Abstractions;
using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ICoverService _coverService;
    private readonly IAuditService _auditService;

    public CoversController(ICoverService coverService, IAuditService auditService)
    {
        _coverService = coverService;
        _auditService = auditService;
    }

    [HttpPost("compute")]
    public ActionResult ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        return Ok(_coverService.ComputePremium(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Cover>>> GetAsync(CancellationToken cancellationToken)
    {
        var covers = await _coverService.GetAllAsync(cancellationToken);
        return Ok(covers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id, CancellationToken cancellationToken)
    {
        var cover = await _coverService.GetByIdAsync(id, cancellationToken);
        return cover is null ? NotFound() : Ok(cover);
    }

    [HttpPost]
    public async Task<ActionResult<Cover>> CreateAsync(Cover cover, CancellationToken cancellationToken)
    {
        var result = await _coverService.CreateAsync(cover, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        await _auditService.AuditCoverAsync(result.Value!.Id, "POST", cancellationToken);
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var deleted = await _coverService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        await _auditService.AuditCoverAsync(id, "DELETE", cancellationToken);
        return NoContent();
    }
}
