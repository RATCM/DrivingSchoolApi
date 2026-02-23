using DrivingSchoolApi.Domain.Entities;

namespace DrivingSchoolApi.Application.Repositories;

public interface IDrivingSchoolRepository
{
    Task<bool> Create(DrivingSchool drivingSchool);
    Task<DrivingSchool?> Get(Guid id);
    Task<IEnumerable<DrivingSchool>> GetAll();
    Task<bool> Update(DrivingSchool drivingSchool);
    Task<bool> Delete(Guid id);
}