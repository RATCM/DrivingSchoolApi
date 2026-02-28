using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class TheoryLessonRepository : Repository, ITheoryLessonRepository
{
    public TheoryLessonRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }

    public async Task<bool> Create(TheoryLesson theoryLesson)
    {
        var entity = await DbContext.TheoryLessons.AddAsync(theoryLesson);

        return entity.State == EntityState.Added;
    }

    public async Task<TheoryLesson?> Get(TheoryLessonKey id)
    {
        return await DbContext.TheoryLessons.FindAsync(id);
    }

    public async Task<IEnumerable<TheoryLesson>> GetAll()
    {
        return DbContext.TheoryLessons;
    }

    public async Task<bool> Update(TheoryLesson theoryLesson)
    {
        // Theory lesson is immutable, so we probably shouldn't actually have this method anyway
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(TheoryLessonKey id)
    {
        var lesson = await DbContext.TheoryLessons.FindAsync(id);
        if (lesson is null) return false;
        var entry = DbContext.TheoryLessons.Remove(lesson);
        return entry.State == EntityState.Deleted;
    }
}