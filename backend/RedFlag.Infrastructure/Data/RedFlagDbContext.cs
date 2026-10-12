using Microsoft.EntityFrameworkCore;
using RedFlag.Core.Entities;

namespace RedFlag.Infrastructure.Data;

public class RedFlagDbContext : DbContext
{
    public RedFlagDbContext(DbContextOptions<RedFlagDbContext> options) : base(options)
    {
    }

    public DbSet<Report> Reports => Set<Report>();
    public DbSet<ReportFlag> ReportFlags => Set<ReportFlag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Report>(e =>
        {
            e.Property(x => x.Source).HasMaxLength(20);
            e.Property(x => x.Level).HasMaxLength(10);
            e.Property(x => x.TextHash).HasMaxLength(64);
            e.HasIndex(x => x.CreatedAt);
            e.HasIndex(x => x.TextHash);
            e.HasMany(x => x.Flags)
             .WithOne(f => f.Report)
             .HasForeignKey(f => f.ReportId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReportFlag>(e =>
        {
            e.Property(x => x.RuleCode).HasMaxLength(50);
            e.Property(x => x.Explanation).HasMaxLength(300);
            e.HasIndex(x => x.RuleCode);
        });
    }
}