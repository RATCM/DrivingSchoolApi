using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class DrivingLessonRepository : Repository, IDrivingLessonRepository
{
    public DrivingLessonRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }
    
    public async Task<bool> Create(DrivingLesson drivingLesson)
    {
        var entity = await DbContext.DrivingLessons.AddAsync(drivingLesson);

        return entity.State == EntityState.Added;
    }

    public async Task<DrivingLesson?> Get(DrivingLessonKey id)
    {
        return await DbContext.DrivingLessons.FindAsync(id);
    }

    public async Task<IEnumerable<DrivingLesson>> GetAll()
    {
        return DbContext.DrivingLessons;
    }

    public async Task<bool> Update(DrivingLesson drivingLesson)
    {
        // Driving lesson is immutable, so we probably shouldn't actually have this method anyway
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(DrivingLessonKey id)
    {
        var drivingLesson = await DbContext.DrivingLessons.FindAsync(id);
        if (drivingLesson is null) return false;
        var entry = DbContext.DrivingLessons.Remove(drivingLesson);
        return entry.State == EntityState.Deleted;
    }
}