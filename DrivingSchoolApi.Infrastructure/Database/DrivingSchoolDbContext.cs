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

    private readonly string? _connectionString;

    internal DrivingSchoolDbContext(DbContextOptions<DrivingSchoolDbContext> options) : base(options) { }
    
    public DrivingSchoolDbContext(IConfiguration configuration)
    {
        _connectionString = configuration["ConnectionStrings:Database"]!;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(_connectionString is not null)
            optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DrivingSchoolDbContext).Assembly);
    }
}