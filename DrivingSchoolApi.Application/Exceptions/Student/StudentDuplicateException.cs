namespace DrivingSchoolApi.Application.Exceptions.Student;

public class StudentDuplicateException : BadRequestException
{
    public StudentDuplicateException(string message) : base(message) { }
}