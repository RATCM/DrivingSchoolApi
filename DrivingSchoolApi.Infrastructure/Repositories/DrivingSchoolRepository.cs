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
        return await DbContext.DrivingSchools.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<IEnumerable<DrivingSchool>> GetAll()
    {
        return DbContext.DrivingSchools;
    }

    public async Task<bool> Update(DrivingSchool drivingSchool)
    {
        var temp = await DbContext.DrivingSchools.FindAsync(drivingSchool.Id);
        if (temp is null) return false;
        
        var packages = temp.Packages.ToList();
        foreach (var package in packages)
            temp.RemovePackage(package);

        var invites = temp.StudentInvites.ToList();
        foreach(var invite in invites)
            temp.RemoveStudentInvite(invite);
        
        if(temp.DrivingSchoolName != drivingSchool.DrivingSchoolName) temp.ChangeName(drivingSchool.DrivingSchoolName);
        if(temp.StreetAddress != drivingSchool.StreetAddress) temp.ChangeAddress(drivingSchool.StreetAddress);
        if(temp.PhoneNumber != drivingSchool.PhoneNumber) temp.ChangePhoneNumber(drivingSchool.PhoneNumber);
        if(temp.WebAddress != drivingSchool.WebAddress) temp.ChangeWebAddress(drivingSchool.WebAddress);
        
        foreach (var package in drivingSchool.Packages)
            temp.AddPackage(package);
        
        foreach(var invite in drivingSchool.StudentInvites)
            temp.AddStudentInvite(invite);
        
        var entry = DbContext.DrivingSchools.Update(temp);
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
