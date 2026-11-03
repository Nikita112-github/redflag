using RedFlag.Core.Entities;

namespace RedFlag.Api.Models;

public class AdminReportDto
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string MaskedText { get; set; } = string.Empty;
    public int Score { get; set; }
    public string Level { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<FlagDto> Flags { get; set; } = new();

    public static AdminReportDto FromReport(Report report) => new()
    {
        Id = report.Id,
        Source = report.Source,
        MaskedText = report.MaskedText,
        Score = report.Score,
        Level = report.Level,
        CreatedAt = report.CreatedAt,
        Flags = report.Flags.Select(FlagDto.FromFlag).ToList()
    };
}