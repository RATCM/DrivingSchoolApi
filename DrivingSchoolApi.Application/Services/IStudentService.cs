using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IStudentService
{
    Task<Student> CreateStudent(Name name, Email email, string password, PhoneNumber phoneNumber, Guid schoolId);
    Task<Student> GetStudentById(Guid id);
    Task<IEnumerable<Student>> GetAllStudentsFromSchool(Guid schoolId);
    Task DeleteStudent(Guid id);
}