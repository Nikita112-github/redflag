using RedFlag.Core.Entities;

namespace RedFlag.Api.Models;

public class FlagDto
{
    public string Code { get; set; } = string.Empty;
    public int Weight { get; set; }
    public string Explanation { get; set; } = string.Empty;

    public static FlagDto FromFlag(ReportFlag flag) => new()
    {
        Code = flag.RuleCode,
        Weight = flag.Weight,
        Explanation = flag.Explanation
    };
}