using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.E2ETest;

public class TestDbContext : DbContext, IDrivingSchoolDbContext
{
    public DbSet<DrivingLesson> DrivingLessons { get; set; }
    public DbSet<Domain.Entities.DrivingSchool> DrivingSchools { get; set; }
    public DbSet<TheoryLesson> TheoryLessons { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<StudentInvite> StudentInvites { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DrivingSchoolDbContext).Assembly);
    }
}