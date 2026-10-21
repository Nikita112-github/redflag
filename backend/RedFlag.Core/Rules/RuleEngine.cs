namespace RedFlag.Core.Rules;

public record AnalysisResult(int Score, string Level, IReadOnlyList<RuleResult> Flags);

public class RuleEngine
{
    private readonly IReadOnlyList<IRule> _rules;

    public RuleEngine(IEnumerable<IRule> rules)
    {
        _rules = rules.ToList();
    }

    public static RuleEngine CreateDefault() => new(new IRule[]
    {
        new FeeRequestRule(),
        new SensitiveDataRule(),
        new ChatOnlyRule(),
        new FreeEmailRule(),
        new UrgencyRule(),
        new UnrealisticPayRule(),
        new NoInterviewRule(),
        new ShortLinkRule()
    });

    public AnalysisResult Analyze(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return new AnalysisResult(0, "Low", new List<RuleResult>());
        }

        var flags = new List<RuleResult>();
        foreach (var rule in _rules)
        {
            var result = rule.Evaluate(text);
            if (result != null)
            {
                flags.Add(result);
            }
        }

        var score = Math.Min(100, flags.Sum(f => f.Weight));
        return new AnalysisResult(score, LevelFor(score), flags);
    }

    public static string LevelFor(int score) =>
        score >= 60 ? "High" : score >= 30 ? "Medium" : "Low";
}