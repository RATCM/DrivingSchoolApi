using DrivingSchoolApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DrivingSchoolApi.Infrastructure.Database;

internal class DrivingSchoolDbContext : DbContext, IDrivingSchoolDbContext
{
    public DbSet<DrivingLesson> DrivingLessons { get; set; }
    public DbSet<DrivingSchool> DrivingSchools { get; set; }
    public DbSet<TheoryLesson> TheoryLessons { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Student> Students { get; set; }

    private readonly string _connectionString;
    
    public DrivingSchoolDbContext(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:Database"]!;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DrivingSchoolDbContext).Assembly);
    }
}