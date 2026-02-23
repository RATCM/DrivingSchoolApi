using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Database;

internal class DrivingSchoolDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DrivingSchoolDbContext).Assembly);
    }
}