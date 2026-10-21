using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class ShortLinkRuleTests
{
    private readonly ShortLinkRule _rule = new();

    [Theory]
    [InlineData("Exclusive government job guaranteed. Pay application fee and exam fee now. 100 percent placement assured. bit.ly/jobseat")]
    [InlineData("Earn from home 50,000 per month! Click tinyurl.com/earnnow and enter your card details to register.")]
    public void Flags_scam_messages(string text)
    {
        var result = _rule.Evaluate(text);

        Assert.NotNull(result);
        Assert.Equal("SHORT_LINK", result!.Code);
        Assert.Equal(10, result.Weight);
    }

    [Theory]
    [InlineData("Hi Nikita, thanks for applying for the Junior .NET Developer role at Acme Software Pvt Ltd. We would like to schedule a technical interview on Thursday at 11 AM via Google Meet. Please confirm your availability.")]
    [InlineData("Hi, this is Priya from the talent team at Orbit Tech (priya@orbittech.example). We saw your profile and have an opening for React Developer, 0-2 years, Pune. Are you open to a quick call this week?")]
    [InlineData("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.")]
    public void Does_not_flag_genuine_or_unrelated_messages(string text)
    {
        Assert.Null(_rule.Evaluate(text));
    }
}