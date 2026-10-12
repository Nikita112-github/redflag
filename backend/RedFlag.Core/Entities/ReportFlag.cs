namespace RedFlag.Core.Entities;

public class ReportFlag
{
    public int Id { get; set; }
    public int ReportId { get; set; }
    public string RuleCode { get; set; } = string.Empty;
    public int Weight { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public Report? Report { get; set; }
}