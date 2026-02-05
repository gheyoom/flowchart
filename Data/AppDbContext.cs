using Microsoft.EntityFrameworkCore;
using FlowChartApp.Models;

namespace FlowChartApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<FlowBox> FlowBoxes { get; set; }
    public DbSet<FlowConnection> FlowConnections { get; set; }
    public DbSet<LegendItem> LegendItems { get; set; }
    public DbSet<AppMetadata> AppMetadata { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlowConnection>()
            .HasOne(c => c.SourceBox)
            .WithMany(b => b.OutgoingConnections)
            .HasForeignKey(c => c.SourceBoxId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FlowConnection>()
            .HasOne(c => c.TargetBox)
            .WithMany()
            .HasForeignKey(c => c.TargetBoxId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
