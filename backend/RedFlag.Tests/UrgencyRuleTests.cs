using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class UrgencyRuleTests
{
    private readonly UrgencyRule _rule = new();

    [Theory]
    [InlineData("Your offer letter from ABC Solutions is attached. Pay Rs 5,000 as security deposit within 2 hours or your seat will be cancelled.")]
    [InlineData("Final call: your joining is confirmed. Transfer Rs 6,500 as background check fee. Offer letter will be sent after payment.")]
    [InlineData("URGENT: only 3 seats left. Pay registration charge in 1 hour. Salary 35,000 for 12th pass, no skills needed.")]
    [InlineData("You have been shortlisted without any application. Training fee Rs 3,000 required. Hurry, last chance!")]
    [InlineData("Act now! Limited vacancies. Deposit Rs 3,500 to hold your position, refundable after joining. Contact only on Telegram.")]
    public void Flags_scam_messages(string text)
    {
        var result = _rule.Evaluate(text);

        Assert.NotNull(result);
        Assert.Equal("URGENCY", result!.Code);
        Assert.Equal(10, result.Weight);
    }

    [Theory]
    [InlineData("Hello, your application for Data Analyst at Brightline Analytics has been received. Our HR team will contact you within 7 working days. No action is needed from your side.")]
    [InlineData("Dear candidate, the written test link for the Associate Software Engineer role is below. The test takes 60 minutes and is free. Please complete it before Sunday.")]
    [InlineData("Thank you for attending the interview. We will share the outcome by next Wednesday. Regards, HR, Zenith Labs.")]
    [InlineData("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.")]
    public void Does_not_flag_genuine_or_unrelated_messages(string text)
    {
        Assert.Null(_rule.Evaluate(text));
    }
}