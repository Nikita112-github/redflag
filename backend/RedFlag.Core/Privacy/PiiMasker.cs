using System.Text.RegularExpressions;

namespace RedFlag.Core.Privacy;

public static class PiiMasker
{
    private static readonly Regex Email = new(@"[\w.+-]+@[\w-]+(?:\.[\w-]+)+", RegexOptions.Compiled);
    private static readonly Regex Aadhaar = new(@"\b\d{4}\s?\d{4}\s?\d{4}\b", RegexOptions.Compiled);
    private static readonly Regex Phone = new(@"(?<!\d)(?:\+91[\s-]?)?[6-9]\d{9}(?!\d)", RegexOptions.Compiled);
    private static readonly Regex Pan = new(@"\b[A-Z]{5}\d{4}[A-Z]\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static string Mask(string text)
    {
        var masked = Email.Replace(text, "[EMAIL]");
        masked = Phone.Replace(masked, "[PHONE]");
        masked = Aadhaar.Replace(masked, "[AADHAAR]");
        masked = Pan.Replace(masked, "[PAN]");
        return masked;
    }
}