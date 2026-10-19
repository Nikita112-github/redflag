using System.Text.RegularExpressions;

namespace RedFlag.Core.Rules;

public class FeeRequestRule : IRule
{
    public string Code => "FEE_REQUEST";
    private const int Weight = 35;

    private const string Amount = @"(?:rs\.?|inr|₹)\s?\d[\d,]*";

    private static Regex Make(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex[] Patterns =
    {
        // "Pay Rs 2,500", "Transfer Rs 6,500", "Deposit Rs 3,500"
        Make(@"\b(?:pay|send|transfer|deposit|remit)\b[^.!?]{0,40}?" + Amount),
        // "Training fee Rs 3,000", "charges Rs 1,800"
        Make(@"\b(?:fee|fees|charge|charges|deposit)\b\s*(?:of|is|:)?\s*" + Amount),
        // "Pay application fee", "Pay registration charge"
        Make(@"\bpay\b[^.!?]{0,30}?\b(?:registration|application|exam|training|security|processing|verification|joining|interview|course|appointment|kit)\b[^.!?]{0,20}?\b(?:fee|fees|charge|charges|deposit|amount)\b"),
        // "Register with us for Rs 2,000"
        Make(@"\b(?:register|registration|enrol|enroll)\b[^.!?]{0,30}?\b(?:for|of)\s*" + Amount),
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
                    "Asks you to pay money (a fee or deposit) as part of hiring. Genuine employers do not charge candidates.");
            }
        }
        return null;
    }
}