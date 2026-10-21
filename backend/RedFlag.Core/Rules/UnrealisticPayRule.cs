using System.Text.RegularExpressions;

namespace RedFlag.Core.Rules;

public class UnrealisticPayRule : IRule
{
    public string Code => "UNREALISTIC_PAY";
    private const int Weight = 15;

    private static Regex Make(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex[] Patterns =
    {
        Make(@"\b\d+(?:\.\d+)?\s*(?:lakh|lakhs|lac)\s*(?:per|a|/)\s*month\b"),
        Make(@"\bearn\b[^.!?]{0,30}?\d[\d,]*\s*(?:per|a|/)?\s*(?:day|daily|week|weekly|month|monthly)\b")
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
                    "Promises very high pay for little or no experience. If it sounds too good to be true, verify it.");
            }
        }
        return null;
    }
}