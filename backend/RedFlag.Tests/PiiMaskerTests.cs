using RedFlag.Core.Privacy;
using Xunit;

namespace RedFlag.Tests;

public class PiiMaskerTests
{
    [Fact]
    public void Masks_phone_email_aadhaar_and_pan()
    {
        var masked = PiiMasker.Mask("Call me on +91 9322357805 or email nikita@example.com. Aadhaar 1234 5678 9012, PAN ABCDE1234F.");

        Assert.Equal("Call me on [PHONE] or email [EMAIL]. Aadhaar [AADHAAR], PAN [PAN].", masked);
    }

    [Fact]
    public void Masks_phone_numbers_with_and_without_country_code()
    {
        var masked = PiiMasker.Mask("WhatsApp 9876543210 or +919876543210 now. Aadhaar 123456789012.");

        Assert.Equal("WhatsApp [PHONE] or [PHONE] now. Aadhaar [AADHAAR].", masked);
    }

    [Fact]
    public void Leaves_normal_text_and_amounts_unchanged()
    {
        var masked = PiiMasker.Mask("Pay Rs 2,500 to confirm your seat.");

        Assert.Equal("Pay Rs 2,500 to confirm your seat.", masked);
    }
}