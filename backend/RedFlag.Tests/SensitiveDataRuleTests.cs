using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class SensitiveDataRuleTests
{
    private readonly SensitiveDataRule _rule = new();

    [Theory]
    [InlineData("Hi, we have urgent openings for freshers. Salary 40,000 per month, work from home, no interview. Send Aadhaar and bank details to join today.")]
    [InlineData("Part-time job: like and review videos, earn Rs 5,000 per day. Send your UPI ID and OTP to receive the first payment.")]
    [InlineData("Congratulations on your selection at LMN Global. Share PAN card, Aadhaar photo and bank passbook for payroll setup before any interview.")]
    [InlineData("Offer letter attached. Reply with your bank account number and OTP so we can verify your salary account today.")]
    [InlineData("Earn from home 50,000 per month! Click tinyurl.com/earnnow and enter your card details to register.")]
    public void Flags_scam_messages(string text)
    {
        var result = _rule.Evaluate(text);

        Assert.NotNull(result);
        Assert.Equal("SENSITIVE_DATA", result!.Code);
        Assert.Equal(30, result.Weight);
    }

    [Theory]
    [InlineData("Your application for QA Analyst is under review. You may be asked to complete a short assignment. Do not share bank or ID details until you receive a formal offer.")]
    [InlineData("Hi, we are hiring Python Developers (1 to 3 years) at Kite Networks. CTC 6 to 9 LPA, hybrid in Hyderabad. Apply on our careers page. Please do not send money or documents over chat.")]
    [InlineData("Hi, this is Priya from the talent team at Orbit Tech (priya@orbittech.example). We saw your profile and have an opening for React Developer, 0-2 years, Pune. Are you open to a quick call this week?")]
    [InlineData("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.")]
    [InlineData("Never share your OTP or bank account number with anyone, including people claiming to be from HR.")]
    [InlineData("Please do not send your Aadhaar or PAN card over WhatsApp. We verify documents in person on joining day.")]
    public void Does_not_flag_genuine_or_unrelated_messages(string text)
    {
        Assert.Null(_rule.Evaluate(text));
    }
}