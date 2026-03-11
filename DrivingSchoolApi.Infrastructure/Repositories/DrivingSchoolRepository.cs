using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class DrivingSchoolRepository : Repository, IDrivingSchoolRepository
{
    public DrivingSchoolRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }

    public async Task<bool> Create(DrivingSchool drivingSchool)
    {
        var entry = await DbContext.DrivingSchools.AddAsync(drivingSchool);

        return entry.State == EntityState.Added;
    }

    public async Task<DrivingSchool?> Get(DrivingSchoolKey id)
    {
        return await DbContext.DrivingSchools.FindAsync(id);
    }
    
    public async Task<IEnumerable<DrivingSchool>> GetAll()
    {
        return DbContext.DrivingSchools;
    }

    public async Task<bool> Update(DrivingSchool drivingSchool)
    {
        var school = await DbContext.DrivingSchools.FindAsync(drivingSchool.Id);
        if (school is null) return false;
        var entry = DbContext.DrivingSchools.Update(drivingSchool);
        return entry.State is EntityState.Modified or EntityState.Unchanged;
    }

    public async Task<bool> Delete(DrivingSchoolKey id)
    {
        var school = await DbContext.DrivingSchools.FindAsync(id);
        if (school is null) return false;
        var entry = DbContext.DrivingSchools.Remove(school);
        return entry.State == EntityState.Deleted;
    }
}