using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class FreeEmailRuleTests
{
    private readonly FreeEmailRule _rule = new();

    [Theory]
    [InlineData("Dear candidate, HR from PQR Infotech here (hr.pqrinfotech@gmail.com). Document verification charges Rs 1,800 must be paid today.")]
    [InlineData("For details contact jobs.hiring2026@yahoo.in today.")]
    public void Flags_scam_messages(string text)
    {
        var result = _rule.Evaluate(text);

        Assert.NotNull(result);
        Assert.Equal("FREE_EMAIL", result!.Code);
        Assert.Equal(10, result.Weight);
    }

    [Theory]
    [InlineData("Hi, this is Priya from the talent team at Orbit Tech (priya@orbittech.example). We saw your profile and have an opening for React Developer, 0-2 years, Pune. Are you open to a quick call this week?")]
    [InlineData("Reminder: your HR discussion is tomorrow at 3 PM on Microsoft Teams. The meeting link is in your calendar invite from careers@lumenworks.example.")]
    [InlineData("Hi Nikita, thanks for applying for the Junior .NET Developer role at Acme Software Pvt Ltd. We would like to schedule a technical interview on Thursday at 11 AM via Google Meet. Please confirm your availability.")]
    [InlineData("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.")]
    public void Does_not_flag_genuine_or_unrelated_messages(string text)
    {
        Assert.Null(_rule.Evaluate(text));
    }
}