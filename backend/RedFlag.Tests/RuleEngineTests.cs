using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class RuleEngineTests
{
    private readonly RuleEngine _engine = RuleEngine.CreateDefault();

    [Fact]
    public void Empty_text_scores_zero()
    {
        var result = _engine.Analyze("   ");

        Assert.Equal(0, result.Score);
        Assert.Equal("Low", result.Level);
        Assert.Empty(result.Flags);
    }

    [Fact]
    public void Fee_request_alone_is_medium_risk()
    {
        var result = _engine.Analyze("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.");

        Assert.Equal(35, result.Score);
        Assert.Equal("Medium", result.Level);
    }

    [Fact]
    public void Several_signals_together_are_high_risk()
    {
        var result = _engine.Analyze("You have been shortlisted without any application. Training fee Rs 3,000 required. Hurry, last chance!");

        Assert.Equal(60, result.Score);
        Assert.Equal("High", result.Level);
        Assert.Contains(result.Flags, f => f.Code == "FEE_REQUEST");
        Assert.Contains(result.Flags, f => f.Code == "URGENCY");
        Assert.Contains(result.Flags, f => f.Code == "NO_INTERVIEW");
    }

    [Fact]
    public void Sensitive_data_request_alone_is_medium_risk()
    {
        var result = _engine.Analyze("Congratulations on your selection at LMN Global. Share PAN card, Aadhaar photo and bank passbook for payroll setup before any interview.");

        Assert.Equal(30, result.Score);
        Assert.Equal("Medium", result.Level);
    }

    [Fact]
    public void One_weak_signal_stays_low()
    {
        var result = _engine.Analyze("Join our Telegram group Freshers Jobs 2026. Interview will be held on chat only. Message HR on WhatsApp to start.");

        Assert.Equal(15, result.Score);
        Assert.Equal("Low", result.Level);
        Assert.Contains(result.Flags, f => f.Code == "CHAT_ONLY");
    }

    [Theory]
    [InlineData("We are pleased to offer you the position of Software Engineer Trainee at Northwind Systems. Your CTC is 4.2 LPA. Please review the attached offer letter and reply by Friday. Joining details will follow after background verification, which the company conducts at its own cost.")]
    [InlineData("Hi, this is Priya from the talent team at Orbit Tech (priya@orbittech.example). We saw your profile and have an opening for React Developer, 0-2 years, Pune. Are you open to a quick call this week?")]
    [InlineData("Hi, we are hiring Python Developers (1 to 3 years) at Kite Networks. CTC 6 to 9 LPA, hybrid in Hyderabad. Apply on our careers page. Please do not send money or documents over chat.")]
    public void Genuine_messages_score_zero(string text)
    {
        var result = _engine.Analyze(text);

        Assert.Equal(0, result.Score);
        Assert.Equal("Low", result.Level);
        Assert.Empty(result.Flags);
    }

    [Fact]
    public void Score_is_capped_at_100()
    {
        var result = _engine.Analyze("Urgent! Pay Rs 999 registration fee within 2 hours. Send your Aadhaar. Contact only on WhatsApp. No interview needed, earn 5,000 per day. Email hr.jobs@gmail.com or open bit.ly/x1");

        Assert.Equal(8, result.Flags.Count);
        Assert.Equal(100, result.Score);
        Assert.Equal("High", result.Level);
    }
}