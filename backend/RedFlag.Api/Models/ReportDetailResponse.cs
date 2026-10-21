using RedFlag.Core.Entities;

namespace RedFlag.Api.Models;

public class ReportDetailResponse : AnalyzeResponse
{
    public string Source { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public static ReportDetailResponse FromReportDetail(Report report) => new()
    {
        ReportId = report.Id,
        Score = report.Score,
        Level = report.Level,
        Flags = report.Flags.Select(FlagDto.FromFlag).ToList(),
        Source = report.Source,
        CreatedAt = report.CreatedAt
    };
}