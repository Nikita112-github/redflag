using Microsoft.EntityFrameworkCore;
using RedFlag.Core.Entities;
using RedFlag.Core.Services;
using RedFlag.Infrastructure.Data;

namespace RedFlag.Infrastructure.Services;

public class AdminStatsService : IAdminStatsService
{
    private readonly RedFlagDbContext _db;

    public AdminStatsService(RedFlagDbContext db)
    {
        _db = db;
    }

    public async Task<AdminStats> GetStatsAsync(CancellationToken cancellationToken = default)
    {
        var total = await _db.Reports.CountAsync(cancellationToken);

        var byLevel = await _db.Reports
            .GroupBy(r => r.Level)
            .Select(g => new LevelCount(g.Key, g.Count()))
            .ToListAsync(cancellationToken);

        var topFlags = await _db.ReportFlags
            .GroupBy(f => f.RuleCode)
            .OrderByDescending(g => g.Count())
            .Take(10)
            .Select(g => new FlagCount(g.Key, g.Count()))
            .ToListAsync(cancellationToken);

        return new AdminStats(total, byLevel, topFlags);
    }

    public async Task<IReadOnlyList<Report>> GetLatestReportsAsync(int count, CancellationToken cancellationToken = default)
    {
        var take = Math.Clamp(count, 1, 100);

        return await _db.Reports
            .Include(r => r.Flags)
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .ThenByDescending(r => r.Id)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}