using Microsoft.EntityFrameworkCore;
using TrialTrack.Models;

namespace TrialTrack.Data;

public class TrialTrackDbContext : DbContext
{
    public TrialTrackDbContext(DbContextOptions<TrialTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<Study> Studies { get; set; }
}