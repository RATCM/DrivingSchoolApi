namespace DrivingSchoolApi.Application.Exceptions.Student;

public class StudentConflictException : BadRequestException
{
    public StudentConflictException(string message) : base(message) { }
}