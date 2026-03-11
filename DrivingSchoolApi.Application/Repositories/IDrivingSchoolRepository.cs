using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.Repositories;

public interface IDrivingSchoolRepository
{
    Task<bool> Create(DrivingSchool drivingSchool);
    Task<DrivingSchool?> Get(DrivingSchoolKey id);
    Task<IEnumerable<DrivingSchool>> GetAll();
    Task<bool> Update(DrivingSchool drivingSchool);
    Task<bool> Delete(DrivingSchoolKey id);
    Task Save();
}