using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IDrivingSchoolService
{
    Task<DrivingSchool> CreateDrivingSchool(DrivingSchoolName name, StreetAddress streetAddress, PhoneNumber phoneNumber, WebAddress webAddress);
    Task<DrivingSchool> GetDrivingSchoolById(DrivingSchoolKey id);
    Task<IEnumerable<DrivingSchool>> GetAllDrivingSchools();
    Task DeleteDrivingSchool(DrivingSchoolKey id);
}