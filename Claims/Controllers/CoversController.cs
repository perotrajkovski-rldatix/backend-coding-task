using Claims.Application.Abstractions;
using Claims.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ICoverService _coverService;
    private readonly Auditer _auditer;

    public CoversController(ICoverService coverService, AuditContext auditContext)
    {
        _coverService = coverService;
        _auditer = new Auditer(auditContext);
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

        _auditer.AuditCover(result.Value!.Id, "POST");
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        _auditer.AuditCover(id, "DELETE");
        var deleted = await _coverService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
