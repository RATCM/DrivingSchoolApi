using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.Services;

public interface IStudentService
{
    Task<Result<Student>> CreateStudent(Name name, 
        Email email, 
        string password, 
        PhoneNumber phoneNumber, 
        DrivingSchoolKey schoolId);
    
    Task<Result<Student>> GetStudentById(StudentKey id);
    Task<Result<IEnumerable<Student>>> GetAllStudents();
    Task<Result<IEnumerable<Student>>> GetAllStudentsFromSchool(DrivingSchoolKey schoolId);
    Task<Result> DeleteStudent(Guid claimedId, StudentKey deleteId, bool isAdmin);
}