using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class InstructorRepository : Repository, IInstructorRepository
{
    public InstructorRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }

    public async Task<bool> Create(Instructor instructor)
    {
        var entry = await DbContext.Instructors.AddAsync(instructor);

        return entry.State == EntityState.Added;
    }

    public async Task<Instructor?> Get(InstructorKey id)
    {
        return await DbContext.Instructors.FindAsync(id);
    }

    public async Task<Instructor?> GetByEmail(Email email)
    {
        return await DbContext.Instructors
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.EmailAddress == email);
    }
    
    public async Task<IEnumerable<Instructor>> GetAll()
    {
        return DbContext.Instructors;
    }

    public async Task<bool> Update(Instructor instructor)
    {
        var temp = await DbContext.Instructors.FindAsync(instructor.Id);
        if (temp is null) return false;
        var entry = DbContext.Instructors.Update(instructor);
        return entry.State is EntityState.Modified or EntityState.Unchanged;
    }

    public async Task<bool> Delete(InstructorKey id)
    {
        var temp = await DbContext.Instructors.FindAsync(id);
        if (temp is null) return false;
        var entry = DbContext.Instructors.Remove(temp);
        return entry.State == EntityState.Deleted;
    }
}