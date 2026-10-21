using Microsoft.EntityFrameworkCore;
using RedFlag.Core.Entities;
using RedFlag.Core.Privacy;
using RedFlag.Core.Rules;
using RedFlag.Core.Services;
using RedFlag.Infrastructure.Data;

namespace RedFlag.Infrastructure.Services;

public class AnalysisService : IAnalysisService
{
    private readonly RuleEngine _engine;
    private readonly RedFlagDbContext _db;

    public AnalysisService(RuleEngine engine, RedFlagDbContext db)
    {
        _engine = engine;
        _db = db;
    }

    public async Task<Report> AnalyzeAndSaveAsync(string text, string source, CancellationToken cancellationToken = default)
    {
        // 1. Rules run on the ORIGINAL text (they need details such as the email domain).
        var analysis = _engine.Analyze(text);

        // 2. Personal data is masked BEFORE anything is stored.
        var maskedText = PiiMasker.Mask(text);

        var report = new Report
        {
            Source = source,
            MaskedText = maskedText,
            TextHash = TextHasher.Hash(maskedText),
            Score = analysis.Score,
            Level = analysis.Level,
            CreatedAt = DateTime.UtcNow,
            Flags = analysis.Flags.Select(f => new ReportFlag
            {
                RuleCode = f.Code,
                Weight = f.Weight,
                Explanation = f.Explanation
            }).ToList()
        };

        _db.Reports.Add(report);
        await _db.SaveChangesAsync(cancellationToken);
        return report;
    }

    public Task<Report?> GetReportAsync(int id, CancellationToken cancellationToken = default) =>
        _db.Reports
            .Include(r => r.Flags)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
}