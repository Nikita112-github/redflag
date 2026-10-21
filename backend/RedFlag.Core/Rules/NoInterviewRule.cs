using System.Text.RegularExpressions;

namespace RedFlag.Core.Rules;

public class NoInterviewRule : IRule
{
    public string Code => "NO_INTERVIEW";
    private const int Weight = 15;

    private static Regex Make(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex[] Patterns =
    {
        Make(@"\bno\s+interview\b"),
        Make(@"\bwithout\s+(?:any\s+)?(?:interview|application|test)\b"),
        Make(@"\bnever\s+took\b"),
        Make(@"\bdirect(?:ly)?\s+(?:selected|selection)\b")
    };

    public RuleResult? Evaluate(string text)
    {
        foreach (var pattern in Patterns)
        {
            if (pattern.IsMatch(text))
            {
                return new RuleResult(
                    Code,
                    Weight,
                    "Offers or selection without a real interview or application. Genuine hiring includes an interview process.");
            }
        }
        return null;
    }
}