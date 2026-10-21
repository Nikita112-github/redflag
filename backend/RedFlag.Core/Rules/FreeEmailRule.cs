using System.Text.RegularExpressions;

namespace RedFlag.Core.Rules;

public class FreeEmailRule : IRule
{
    public string Code => "FREE_EMAIL";
    private const int Weight = 10;

    private static Regex Make(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex[] Patterns =
    {
        Make(@"[\w.+-]+@(?:gmail|yahoo|outlook|hotmail|rediffmail|ymail|protonmail|proton)\.(?:com|in|co\.in|me)\b")
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
                    "The recruiter uses a free email address (Gmail, Yahoo, Outlook). Genuine companies usually email from their own domain.");
            }
        }
        return null;
    }
}