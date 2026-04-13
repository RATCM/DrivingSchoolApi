using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IInstructorService
{
    Task<Result<Instructor>> CreateInstructor(Name name, 
        Email email, 
        string password, 
        PhoneNumber phoneNumber, 
        DrivingSchoolKey schoolId);

    Task<Result<(string AccessToken, string RefreshToken)>> LoginAsInstructor(string email, string password);
    Task<Result<IEnumerable<Instructor>>> GetAllInstructors();
    Task<Result<Instructor>> GetInstructorById(InstructorKey id);
    Task<Result<IEnumerable<Instructor>>> GetAllInstructorsFromSchool(DrivingSchoolKey schoolId);
    Task<Result<DrivingSchoolKey>> GetInstructorDrivingSchoolId(InstructorKey id);
    Task<Result<Instructor>> UpdateInstructor(
        InstructorKey id,
        DrivingSchoolKey schoolId,
        Name name,
        Email email,
        PhoneNumber phoneNumber);
    Task<Result<Instructor>> UpdateInstructorPassword(
        InstructorKey id,
        string oldPassword,
        string newPassword);
    Task<Result> DeleteInstructor(InstructorKey id);
}
