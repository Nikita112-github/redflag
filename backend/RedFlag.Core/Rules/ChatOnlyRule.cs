using System.Text.RegularExpressions;

namespace RedFlag.Core.Rules;

public class ChatOnlyRule : IRule
{
    public string Code => "CHAT_ONLY";
    private const int Weight = 15;

    private static Regex Make(string pattern) =>
        new(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

    private static readonly Regex[] Patterns =
    {
        Make(@"\b(?:interview|selection|process)\b[^.!?]{0,40}?\b(?:on|via|through|over)\s+(?:chat|whatsapp|telegram)\b"),
        Make(@"\bonly\b[^.!?]{0,20}?\b(?:whatsapp|telegram|chat)\b"),
        Make(@"\b(?:whatsapp|telegram|chat)\s+only\b"),
        Make(@"\bjoin\b[^.!?]{0,30}?\b(?:whatsapp|telegram)\b[^.!?]{0,10}?\bgroup\b")
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
                    "Hiring steps happen only on WhatsApp or Telegram chat. Genuine employers use official email, calls or video interviews.");
            }
        }
        return null;
    }
}