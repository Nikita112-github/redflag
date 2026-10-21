using Microsoft.AspNetCore.Mvc;
using RedFlag.Api.Models;
using RedFlag.Core.Services;

namespace RedFlag.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyzeController : ControllerBase
{
    private readonly IAnalysisService _analysisService;

    public AnalyzeController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpPost]
    public async Task<ActionResult<AnalyzeResponse>> Analyze(AnalyzeRequest request, CancellationToken cancellationToken)
    {
        var report = await _analysisService.AnalyzeAndSaveAsync(request.Text, "Web", cancellationToken);
        return Ok(AnalyzeResponse.FromReport(report));
    }
}