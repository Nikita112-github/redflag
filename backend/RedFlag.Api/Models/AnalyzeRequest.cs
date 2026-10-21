using System.ComponentModel.DataAnnotations;

namespace RedFlag.Api.Models;

public class AnalyzeRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 10)]
    public string Text { get; set; } = string.Empty;
}