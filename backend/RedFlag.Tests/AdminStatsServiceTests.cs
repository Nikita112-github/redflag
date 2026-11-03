using Microsoft.EntityFrameworkCore;
using RedFlag.Core.Rules;
using RedFlag.Infrastructure.Data;
using RedFlag.Infrastructure.Services;
using Xunit;

namespace RedFlag.Tests;

public class AdminStatsServiceTests
{
    private const string ScamMessage = "Pay Rs 2,500 registration fee. Call 9322357805 or mail hr@gmail.com";
    private const string GenuineMessage = "Hi, this is Priya from the talent team at Orbit Tech (priya@orbittech.example). We saw your profile and have an opening for React Developer, 0-2 years, Pune. Are you open to a quick call this week?";

    private static RedFlagDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<RedFlagDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new RedFlagDbContext(options);
    }

    [Fact]
    public async Task Stats_count_reports_by_level_and_top_flags()
    {
        using var db = CreateDb();
        var analysis = new AnalysisService(RuleEngine.CreateDefault(), db);
        await analysis.AnalyzeAndSaveAsync(ScamMessage, "Web");
        await analysis.AnalyzeAndSaveAsync(GenuineMessage, "Web");

        var stats = await new AdminStatsService(db).GetStatsAsync();

        Assert.Equal(2, stats.TotalReports);
        Assert.Contains(stats.ByLevel, l => l.Level == "Medium" && l.Count == 1);
        Assert.Contains(stats.ByLevel, l => l.Level == "Low" && l.Count == 1);
        Assert.Contains(stats.TopFlags, f => f.Code == "FEE_REQUEST" && f.Count == 1);
    }

    [Fact]
    public async Task Latest_reports_are_newest_first_and_limited_by_count()
    {
        using var db = CreateDb();
        var analysis = new AnalysisService(RuleEngine.CreateDefault(), db);
        await analysis.AnalyzeAndSaveAsync(ScamMessage, "Web");
        await analysis.AnalyzeAndSaveAsync(GenuineMessage, "Web");
        await analysis.AnalyzeAndSaveAsync(ScamMessage, "Web");

        var latest = await new AdminStatsService(db).GetLatestReportsAsync(2);

        Assert.Equal(2, latest.Count);
        Assert.Equal(3, latest[0].Id);
        Assert.Equal(2, latest[1].Id);
    }
}