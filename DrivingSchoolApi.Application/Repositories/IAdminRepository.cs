using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Repositories;

public interface IAdminRepository
{
    Task<bool> Create(Admin admin);
    Task<Admin?> Get(AdminKey id);
    Task<Admin?> GetByEmail(Email email);
    Task<IEnumerable<Admin>> GetAll();
    Task<bool> Update(Admin admin);
    Task<bool> Delete(AdminKey id);
    Task Save();
}