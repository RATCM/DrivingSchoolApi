using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IStudentService
{
    Task<Student> CreateStudent(Name name, 
        Email email, 
        string password, 
        PhoneNumber phoneNumber, 
        DrivingSchoolKey schoolId);
    Task<Student> GetStudentById(StudentKey id);
    Task<IEnumerable<Student>> GetAllStudentsFromSchool(DrivingSchoolKey schoolId);
    Task DeleteStudent(StudentKey id);
}