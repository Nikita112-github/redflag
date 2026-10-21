using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class UnrealisticPayRuleTests
{
    private readonly UnrealisticPayRule _rule = new();

    [Theory]
    [InlineData("Part-time job: like and review videos, earn Rs 5,000 per day. Send your UPI ID and OTP to receive the first payment.")]
    [InlineData("Selected as Software Developer at 1 lakh per month for freshers. No experience needed. Pay Rs 4,000 for laptop kit and ID card.")]
    [InlineData("We are hiring for work from home. Earn 30,000 weekly. Join WhatsApp group, no interview, immediate joining.")]
    [InlineData("Earn from home 50,000 per month! Click tinyurl.com/earnnow and enter your card details to register.")]
    [InlineData("Your visa job offer abroad is confirmed. Pay Rs 80,000 processing fee. Salary 3 lakh per month, no experience.")]
    public void Flags_scam_messages(string text)
    {
        var result = _rule.Evaluate(text);

        Assert.NotNull(result);
        Assert.Equal("UNREALISTIC_PAY", result!.Code);
        Assert.Equal(15, result.Weight);
    }

    [Theory]
    [InlineData("Hi, we are hiring Python Developers (1 to 3 years) at Kite Networks. CTC 6 to 9 LPA, hybrid in Hyderabad. Apply on our careers page. Please do not send money or documents over chat.")]
    [InlineData("Congratulations! After your interviews, we are happy to offer you the Full Stack Developer internship at Harbor Soft, stipend 15,000 per month for 6 months. Offer letter attached; please sign and return.")]
    [InlineData("We are pleased to offer you the position of Software Engineer Trainee at Northwind Systems. Your CTC is 4.2 LPA. Please review the attached offer letter and reply by Friday. Joining details will follow after background verification, which the company conducts at its own cost.")]
    [InlineData("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.")]
    public void Does_not_flag_genuine_or_unrelated_messages(string text)
    {
        Assert.Null(_rule.Evaluate(text));
    }
}