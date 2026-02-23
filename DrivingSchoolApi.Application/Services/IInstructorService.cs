using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IInstructorService
{
    Task<Instructor> CreateInstructor(Name name, Email email, string password, PhoneNumber phoneNumber, Guid schoolId);
    Task<Instructor> GetInstructorById(Guid id);
    Task<IEnumerable<Instructor>> GetAllInstructorsFromSchool(Guid schoolId);
    Task DeleteInstructor(Guid id);
}