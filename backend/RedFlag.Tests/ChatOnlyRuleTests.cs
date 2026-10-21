using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class ChatOnlyRuleTests
{
    private readonly ChatOnlyRule _rule = new();

    [Theory]
    [InlineData("Join our Telegram group Freshers Jobs 2026. Interview will be held on chat only. Message HR on WhatsApp to start.")]
    [InlineData("We are hiring for work from home. Earn 30,000 weekly. Join WhatsApp group, no interview, immediate joining.")]
    [InlineData("Hiring freshers for customer support, 28,000 salary, joining in 2 days. No interview, only WhatsApp chat. Send documents now.")]
    [InlineData("Act now! Limited vacancies. Deposit Rs 3,500 to hold your position, refundable after joining. Contact only on Telegram.")]
    public void Flags_scam_messages(string text)
    {
        var result = _rule.Evaluate(text);

        Assert.NotNull(result);
        Assert.Equal("CHAT_ONLY", result!.Code);
        Assert.Equal(15, result.Weight);
    }

    [Theory]
    [InlineData("Hi, we are hiring Python Developers (1 to 3 years) at Kite Networks. CTC 6 to 9 LPA, hybrid in Hyderabad. Apply on our careers page. Please do not send money or documents over chat.")]
    [InlineData("Reminder: your HR discussion is tomorrow at 3 PM on Microsoft Teams. The meeting link is in your calendar invite from careers@lumenworks.example.")]
    [InlineData("Hi Nikita, thanks for applying for the Junior .NET Developer role at Acme Software Pvt Ltd. We would like to schedule a technical interview on Thursday at 11 AM via Google Meet. Please confirm your availability.")]
    [InlineData("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.")]
    public void Does_not_flag_genuine_or_unrelated_messages(string text)
    {
        Assert.Null(_rule.Evaluate(text));
    }
}