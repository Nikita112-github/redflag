using System.Text.RegularExpressions;

namespace RedFlag.Core.Rules;

public class UrgencyRule : IRule
{
    public string Code => "URGENCY";
    private const int Weight = 10;

    private static Regex Make(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex[] Patterns =
    {
        Make(@"\bwithin\s+\d+\s*(?:hours?|hrs?|minutes?|mins?)\b"),
        Make(@"\b(?:hurry|last chance|final call|act now|urgent(?:ly)?)\b"),
        Make(@"\b(?:only\s+\d+\s+(?:seats?|vacancies|positions?)\s+left|limited\s+(?:seats?|vacancies))\b"),
        Make(@"\b(?:seat|offer|position)\s+will\s+be\s+(?:cancelled|canceled|withdrawn)\b")
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
                    "Pressures you to act fast. Scammers use urgency so you do not stop to verify.");
            }
        }
        return null;
    }
}