using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class CompletedCourseRepository : Repository, ICompletedCourseRepository
{
    public CompletedCourseRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }

    public async Task<bool> Create(CompletedCourse course)
    {
        var entry = await DbContext.CompletedCourses.AddAsync(course);

        return entry.State == EntityState.Added;
    }

    public async Task<CompletedCourse?> Get(CompletedCourseKey id)
    {
        return await DbContext.CompletedCourses.FindAsync(id);
    }

    public async Task<IEnumerable<CompletedCourse>> GetAll()
    {
        return await DbContext.CompletedCourses.ToListAsync();
    }
    
    public async Task<bool> Delete(CompletedCourseKey id)
    {
        var temp = await DbContext.CompletedCourses.FindAsync(id);
        if (temp is null) return false;
        var entry = DbContext.CompletedCourses.Remove(temp);
        return entry.State == EntityState.Deleted;
    }
}