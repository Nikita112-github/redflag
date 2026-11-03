using RedFlag.Core.Entities;

namespace RedFlag.Core.Services;

public record LevelCount(string Level, int Count);

public record FlagCount(string Code, int Count);

public record AdminStats(int TotalReports, IReadOnlyList<LevelCount> ByLevel, IReadOnlyList<FlagCount> TopFlags);

public interface IAdminStatsService
{
    Task<AdminStats> GetStatsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Report>> GetLatestReportsAsync(int count, CancellationToken cancellationToken = default);
}