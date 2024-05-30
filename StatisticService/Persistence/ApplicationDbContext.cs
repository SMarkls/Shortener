using Microsoft.EntityFrameworkCore;
using StatisticService.Models;

namespace StatisticService.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Statistic> Statistics { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}
