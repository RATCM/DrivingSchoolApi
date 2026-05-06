using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class StudentInviteRepository : Repository, IStudentInviteRepository
{
    public StudentInviteRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }
    
    public async Task<StudentInvite?> Get(StudentInviteKey id)
    {
        return await DbContext.DrivingSchools.AsNoTracking()
            .SelectMany(x => x.StudentInvites)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<IEnumerable<StudentInvite>> GetAll()
    {
        return await DbContext.DrivingSchools.AsNoTracking()
            .SelectMany(x => x.StudentInvites)
            .ToListAsync();
    }
    
    public async Task<bool> Delete(StudentInviteKey id)
    {
        var temp = await DbContext.DrivingSchools.AsNoTracking()
            .SelectMany(x => x.StudentInvites)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (temp is null) return false;
        var school = await DbContext.DrivingSchools.FindAsync(temp.DrivingSchoolId);
        if (school is null) return false;
        school.RemoveStudentInvite(temp);

        var entry = DbContext.DrivingSchools.Update(school);
        return entry.State == EntityState.Modified;
    }
}
