using RedFlag.Core.Entities;

namespace RedFlag.Core.Services;

public interface IAnalysisService
{
    Task<Report> AnalyzeAndSaveAsync(string text, string source, CancellationToken cancellationToken = default);

    Task<Report?> GetReportAsync(int id, CancellationToken cancellationToken = default);
}