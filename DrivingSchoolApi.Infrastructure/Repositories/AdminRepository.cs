using DrivingSchoolApi.Application.Repositories;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Infrastructure.Database;

namespace DrivingSchoolApi.Infrastructure.Repositories;

internal class AdminRepository : Repository, IAdminRepository
{
    public AdminRepository(IDrivingSchoolDbContext dbContext) : base(dbContext) { }
    
    public async Task<bool> Create(Admin admin)
    {
        // Don't implement this without good reason
        throw new NotImplementedException();
    }

    public async Task<Admin?> Get(AdminKey id)
    {
        return await DbContext.Admins.FindAsync(id);
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
        // Don't implement this without good reason
        throw new NotImplementedException();
    }
}