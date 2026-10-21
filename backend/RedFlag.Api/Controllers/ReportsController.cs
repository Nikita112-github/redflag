using Microsoft.AspNetCore.Mvc;
using RedFlag.Api.Models;
using RedFlag.Core.Services;

namespace RedFlag.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IAnalysisService _analysisService;

    public ReportsController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReportDetailResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var report = await _analysisService.GetReportAsync(id, cancellationToken);
        if (report == null)
        {
            return NotFound();
        }

        return Ok(ReportDetailResponse.FromReportDetail(report));
    }
}