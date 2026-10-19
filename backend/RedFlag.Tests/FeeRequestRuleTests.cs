using RedFlag.Core.Rules;
using Xunit;

namespace RedFlag.Tests;

public class FeeRequestRuleTests
{
    private readonly FeeRequestRule _rule = new();

    [Theory]
    [InlineData("Congratulations! You are selected for Data Entry at XYZ Technologies. Pay Rs 2,500 registration fee to confirm your seat. Fee is refundable in first salary.")]
    [InlineData("Your offer letter from ABC Solutions is attached. Pay Rs 5,000 as security deposit within 2 hours or your seat will be cancelled.")]
    [InlineData("Final call: your joining is confirmed. Transfer Rs 6,500 as background check fee. Offer letter will be sent after payment.")]
    [InlineData("Your resume was selected from a job portal. To schedule your interview, pay Rs 999 interview scheduling fee.")]
    [InlineData("Hello, this is a recruiter from Global Hire Consultancy. Register with us for Rs 2,000 and we will arrange guaranteed jobs.")]
    [InlineData("Dear applicant, your appointment letter is ready. Send Rs 7,500 as appointment letter issuing fee to collect it.")]
    public void Flags_messages_that_demand_money(string text)
    {
        var result = _rule.Evaluate(text);

        Assert.NotNull(result);
        Assert.Equal("FEE_REQUEST", result!.Code);
        Assert.Equal(35, result.Weight);
    }

    [Theory]
    [InlineData("Hi, we have urgent openings for freshers. Salary 40,000 per month, work from home, no interview. Send Aadhaar and bank details to join today.")]
    [InlineData("Hi Nikita, thanks for applying for the Junior .NET Developer role at Acme Software Pvt Ltd. We would like to schedule a technical interview on Thursday at 11 AM via Google Meet. Please confirm your availability.")]
    [InlineData("Hi, we are hiring Python Developers (1 to 3 years) at Kite Networks. CTC 6 to 9 LPA, hybrid in Hyderabad. Apply on our careers page. Please do not send money or documents over chat.")]
    [InlineData("Your joining date is 3 November. On day one, please bring original certificates for verification. The company does not charge any fee at any stage of hiring.")]
    [InlineData("Hi, Neha from Cobalt Systems. We have a walk-in drive for freshers on Saturday, 10 AM to 2 PM, at our Mumbai office. Bring your resume. Registration at venue is free.")]
    public void Does_not_flag_genuine_or_unrelated_messages(string text)
    {
        Assert.Null(_rule.Evaluate(text));
    }
}