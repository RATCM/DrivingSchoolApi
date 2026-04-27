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
    
    Task<Result<(string AccessToken, string RefreshToken)>> LoginAsStudent(string email, string password);
    Task<Result<Student>> GetStudentById(StudentKey id);
    Task<Result<IEnumerable<Student>>> GetAllStudents();
    Task<Result<IEnumerable<Student>>> GetAllStudentsFromSchool(DrivingSchoolKey schoolId);
    Task<Result<DrivingSchoolKey>> GetStudentDrivingSchoolId(StudentKey id);
    Task<Result<Student>> UpdateStudent(StudentKey id, Name name, Email email, PhoneNumber phoneNumber);
    Task<Result> UpdateStudentPassword(StudentKey id, string oldPassword, string newPassword);
    Task<Result<TimeSlot>> AddTimeSlotToCalender(StudentKey id, TimeSlot timeSlot);
    Task<Result<TimeSlot>> RemoveTimeSlotFromCalender(StudentKey id, TimeSlot timeSlot);
    Task<Result> DeleteStudent(StudentKey id);
}
