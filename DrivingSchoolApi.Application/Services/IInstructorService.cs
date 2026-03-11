using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IInstructorService
{
    Task<Instructor> CreateInstructor(Name name, 
        Email email, 
        string password, 
        PhoneNumber phoneNumber, 
        DrivingSchoolKey schoolId);
    Task<Instructor> GetInstructorById(InstructorKey id);
    Task<IEnumerable<Instructor>> GetAllInstructorsFromSchool(DrivingSchoolKey schoolId);
    Task DeleteInstructor(InstructorKey id);
}