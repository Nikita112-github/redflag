namespace RedFlag.Core.Entities;

public class Report
{
    public int Id { get; set; }
    public string Source { get; set; } = "Web";
    public string MaskedText { get; set; } = string.Empty;
    public string TextHash { get; set; } = string.Empty;
    public int Score { get; set; }
    public string Level { get; set; } = "Low";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<ReportFlag> Flags { get; set; } = new();
}