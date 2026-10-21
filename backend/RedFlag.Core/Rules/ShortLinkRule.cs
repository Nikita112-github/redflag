using System.Text.RegularExpressions;

namespace RedFlag.Core.Rules;

public class ShortLinkRule : IRule
{
    public string Code => "SHORT_LINK";
    private const int Weight = 10;

    private static Regex Make(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex[] Patterns =
    {
        Make(@"\b(?:bit\.ly|tinyurl\.com|goo\.gl|cutt\.ly|rb\.gy|is\.gd|shorturl\.at|ow\.ly|t\.ly)/\S+")
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
                    "Contains a shortened link that hides the real website. Do not enter personal details through it.");
            }
        }
        return null;
    }
}