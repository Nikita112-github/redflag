using RedFlag.Core.Entities;

namespace RedFlag.Api.Models;

public class AnalyzeResponse
{
    public int ReportId { get; set; }
    public int Score { get; set; }
    public string Level { get; set; } = string.Empty;
    public List<FlagDto> Flags { get; set; } = new();

    public string Disclaimer { get; set; } =
        "This is a risk score, not a verdict. Verify the employer through official channels before paying or sharing documents.";

    public static AnalyzeResponse FromReport(Report report) => new()
    {
        ReportId = report.Id,
        Score = report.Score,
        Level = report.Level,
        Flags = report.Flags.Select(FlagDto.FromFlag).ToList()
    };
}