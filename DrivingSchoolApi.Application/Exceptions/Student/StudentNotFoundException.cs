namespace DrivingSchoolApi.Application.Exceptions.Student;

public class StudentNotFoundException : NotFoundException
{
    public StudentNotFoundException(string message) : base(message) { }
}