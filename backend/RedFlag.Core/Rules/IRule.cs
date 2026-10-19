namespace RedFlag.Core.Rules;

public interface IRule
{
    string Code { get; }
    RuleResult? Evaluate(string text);
}