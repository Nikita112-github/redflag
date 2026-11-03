using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedFlag.Api.Models;
using RedFlag.Core.Services;

namespace RedFlag.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminStatsService _stats;

    public AdminController(IAdminStatsService stats)
    {
        _stats = stats;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<AdminStats>> GetStats(CancellationToken cancellationToken)
    {
        return Ok(await _stats.GetStatsAsync(cancellationToken));
    }

    [HttpGet("reports/latest")]
    public async Task<ActionResult<List<AdminReportDto>>> GetLatest(int count = 10, CancellationToken cancellationToken = default)
    {
        var reports = await _stats.GetLatestReportsAsync(count, cancellationToken);
        return Ok(reports.Select(AdminReportDto.FromReport).ToList());
    }
}