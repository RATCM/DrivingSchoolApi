using DrivingSchoolApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Database;

internal interface IDrivingSchoolDbContext
{
    DbSet<DrivingLesson> DrivingLessons { get; set; }
    DbSet<DrivingSchool> DrivingSchools { get; set; }
    DbSet<TheoryLesson> TheoryLessons { get; set; }
    DbSet<Instructor> Instructors { get; set; }
    DbSet<Student> Students { get; set; }
    DbSet<Admin> Admins { get; set; }
    DbSet<StudentInvite> StudentInvites { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}