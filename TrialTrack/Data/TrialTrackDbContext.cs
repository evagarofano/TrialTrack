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
    public DbSet<Site> Sites { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Study>()
            .HasIndex(study => study.ProtocolNumber)
            .IsUnique();
        
        modelBuilder.Entity<Subject>()
            .HasIndex(subject => subject.SubjectNumber)
            .IsUnique();
    }

}