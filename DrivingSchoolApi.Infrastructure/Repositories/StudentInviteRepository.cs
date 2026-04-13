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
        return await DbContext.StudentInvites.FindAsync(id);
    }
    
    public async Task<bool> Delete(StudentInviteKey id)
    {
        var temp = await DbContext.StudentInvites.FindAsync(id);
        if (temp is null) return false;
        var entry = DbContext.StudentInvites.Remove(temp);
        return entry.State == EntityState.Deleted;
    }
}