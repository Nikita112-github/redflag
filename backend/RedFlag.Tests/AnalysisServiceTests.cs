using Microsoft.EntityFrameworkCore;
using RedFlag.Core.Rules;
using RedFlag.Infrastructure.Data;
using RedFlag.Infrastructure.Services;
using Xunit;

namespace RedFlag.Tests;

public class AnalysisServiceTests
{
    private static RedFlagDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<RedFlagDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RedFlagDbContext(options);
    }

    [Fact]
    public async Task Saves_masked_text_and_flags_but_scores_the_original_text()
    {
        using var db = CreateDb();
        var service = new AnalysisService(RuleEngine.CreateDefault(), db);

        var report = await service.AnalyzeAndSaveAsync("Pay Rs 2,500 registration fee. Call 9322357805 or mail hr@gmail.com", "Web");

        Assert.True(report.Id > 0);
        Assert.Equal(45, report.Score);
        Assert.Equal("Medium", report.Level);
        Assert.DoesNotContain("9322357805", report.MaskedText);
        Assert.DoesNotContain("hr@gmail.com", report.MaskedText);
        Assert.Contains("[PHONE]", report.MaskedText);
        Assert.Contains("[EMAIL]", report.MaskedText);
        Assert.Equal(64, report.TextHash.Length);
        Assert.Equal(1, await db.Reports.CountAsync());
        Assert.Contains(report.Flags, f => f.RuleCode == "FEE_REQUEST");
        Assert.Contains(report.Flags, f => f.RuleCode == "FREE_EMAIL");
    }

    [Fact]
    public async Task Genuine_message_is_saved_with_low_score_and_no_flags()
    {
        using var db = CreateDb();
        var service = new AnalysisService(RuleEngine.CreateDefault(), db);

        var report = await service.AnalyzeAndSaveAsync("Hi, this is Priya from the talent team at Orbit Tech (priya@orbittech.example). We saw your profile and have an opening for React Developer, 0-2 years, Pune. Are you open to a quick call this week?", "Web");

        Assert.Equal(0, report.Score);
        Assert.Equal("Low", report.Level);
        Assert.Empty(report.Flags);
        Assert.Contains("[EMAIL]", report.MaskedText);
    }

    [Fact]
    public async Task Saved_report_can_be_loaded_with_its_flags()
    {
        using var db = CreateDb();
        var service = new AnalysisService(RuleEngine.CreateDefault(), db);
        var saved = await service.AnalyzeAndSaveAsync("Pay Rs 2,500 registration fee. Call 9322357805 or mail hr@gmail.com", "Web");

        var loaded = await service.GetReportAsync(saved.Id);

        Assert.NotNull(loaded);
        Assert.Equal(saved.Score, loaded!.Score);
        Assert.NotEmpty(loaded.Flags);
    }

    [Fact]
    public async Task Missing_report_returns_null()
    {
        using var db = CreateDb();
        var service = new AnalysisService(RuleEngine.CreateDefault(), db);

        Assert.Null(await service.GetReportAsync(9999));
    }
}