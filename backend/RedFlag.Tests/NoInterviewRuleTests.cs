using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class NoInterviewRuleTests
{
    private readonly NoInterviewRule _rule = new();

    [Theory]
    [InlineData("You have been shortlisted without any application. Training fee Rs 3,000 required. Hurry, last chance!")]
    [InlineData("We are hiring for work from home. Earn 30,000 weekly. Join WhatsApp group, no interview, immediate joining.")]
    [InlineData("Hiring freshers for customer support, 28,000 salary, joining in 2 days. No interview, only WhatsApp chat. Send documents now.")]
    [InlineData("Congrats, you cleared the online test you never took. Pay Rs 2,200 exam result fee to download your offer.")]
    public void Flags_scam_messages(string text)
    {
        var result = _rule.Evaluate(text);

        Assert.NotNull(result);
        Assert.Equal("NO_INTERVIEW", result!.Code);
        Assert.Equal(15, result.Weight);
    }

    [Theory]
    [InlineData("Hi Nikita, thanks for applying for the Junior .NET Developer role at Acme Software Pvt Ltd. We would like to schedule a technical interview on Thursday at 11 AM via Google Meet. Please confirm your availability.")]
    [InlineData("Your interview round 2 is scheduled for Monday 10 AM at our Nagpur office. Please carry a photo ID and your resume. Reply to confirm.")]
    [InlineData("We are inviting you to a campus-style coding contest, free to participate. Top 10 candidates will be interviewed by our engineering team.")]
    [InlineData("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.")]
    public void Does_not_flag_genuine_or_unrelated_messages(string text)
    {
        Assert.Null(_rule.Evaluate(text));
    }
}