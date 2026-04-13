using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;
using DrivingSchoolApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class AdminRepository : Repository, IAdminRepository
{
    public AdminRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }
    
    public async Task<bool> Create(Admin admin)
    {
        var entry = await DbContext.Admins.AddAsync(admin);
        
        return entry.State == EntityState.Added;
    }

    public async Task<Admin?> Get(AdminKey id)
    {
        return await DbContext.Admins.FindAsync(id);
    }

    public async Task<Admin?> GetByEmail(Email email)
    {
        var admins = await DbContext.Admins.ToListAsync();
        return await DbContext.Admins.FirstOrDefaultAsync(x => x.EmailAddress.Equals(email));
    }
    
    public async Task<IEnumerable<Admin>> GetAll()
    {
        // Don't implement this without good reason
        throw new NotImplementedException();
    }

    public async Task<bool> Update(Admin admin)
    {
        // Don't implement this without good reason
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(AdminKey id)
    {
        var temp = await DbContext.Admins.FindAsync(id);
        if (temp is null) return false;
        var entry = DbContext.Admins.Remove(temp);
        return entry.State == EntityState.Deleted;
    }
}