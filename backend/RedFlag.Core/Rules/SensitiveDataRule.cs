using System.Text.RegularExpressions;

namespace RedFlag.Core.Rules;

public class SensitiveDataRule : IRule
{
    public string Code => "SENSITIVE_DATA";
    private const int Weight = 30;

    private static readonly Regex Request = new(
        @"\b(?:send|share|provide|submit|give|enter|upload|forward|reply with)\b[^.!?]{0,60}?(?:aadhaar|aadhar|\bpan (?:card|number)|bank (?:account|details|passbook|statement)|passbook|\botp\b|\bupi (?:id|pin)\b|card details|\bcvv\b)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Ignores safety advice such as "never share your OTP" or "do not send your Aadhaar".
    private static readonly Regex Negation = new(
        @"\b(?:not|never|don't|dont)\s*$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public RuleResult? Evaluate(string text)
    {
        foreach (Match match in Request.Matches(text))
        {
            var start = Math.Max(0, match.Index - 20);
            var before = text.Substring(start, match.Index - start);
            if (Negation.IsMatch(before))
            {
                continue;
            }

            return new RuleResult(
                Code,
                Weight,
                "Asks for sensitive details such as Aadhaar, PAN, bank details, UPI or OTP. Genuine employers do not ask for these before hiring.");
        }
        return null;
    }
}