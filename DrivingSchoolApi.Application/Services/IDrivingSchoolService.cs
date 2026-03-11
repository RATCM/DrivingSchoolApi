using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IDrivingSchoolService
{
    Task<Result<DrivingSchool>> CreateDrivingSchool(DrivingSchoolName name, Address address, PhoneNumber phoneNumber, WebAddress webAddress);
    Task<Result<DrivingSchool>> GetDrivingSchoolById(DrivingSchoolKey id);
    Task<Result<IEnumerable<DrivingSchool>>> GetAllDrivingSchools();
    Task<Result> DeleteDrivingSchool(DrivingSchoolKey id);
}