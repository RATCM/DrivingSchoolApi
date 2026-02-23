using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IDrivingSchoolService
{
    Task<DrivingSchool> CreateDrivingSchool(Address address, PhoneNumber phoneNumber, WebAddress webAddress);
    Task<DrivingSchool> GetDrivingSchoolById(Guid id);
    Task<IEnumerable<DrivingSchool>> GetAllDrivingSchools();
    Task DeleteDrivingSchool(Guid id);
}