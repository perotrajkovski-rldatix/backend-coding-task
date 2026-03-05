using Claims.Application.Abstractions;
using Claims.Application.Abstractions.Auditing;
using Claims.DTOs;
using Claims.DTOs.Covers;
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
    public async Task<ActionResult<IReadOnlyList<CoverResponseDto>>> GetAsync(CancellationToken cancellationToken)
    {
        var covers = await _coverService.GetAllAsync(cancellationToken);
        return Ok(covers.Select(x => x.ToResponse()).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CoverResponseDto>> GetAsync(string id, CancellationToken cancellationToken)
    {
        var cover = await _coverService.GetByIdAsync(id, cancellationToken);
        return cover is null ? NotFound() : Ok(cover.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<CoverResponseDto>> CreateAsync(CreateCoverRequestDto request, CancellationToken cancellationToken)
    {
        var cover = request.ToDomain();
        var result = await _coverService.CreateAsync(cover, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        await _auditService.AuditCoverAsync(result.Value!.Id, "POST");
        return Ok(result.Value.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var deleted = await _coverService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        await _auditService.AuditCoverAsync(id, "DELETE");
        return NoContent();
    }
}
