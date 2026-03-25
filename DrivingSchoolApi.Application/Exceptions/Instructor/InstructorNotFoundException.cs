namespace DrivingSchoolApi.Application.Exceptions.Instructor;

public class InstructorNotFoundException : NotFoundException
{
    public InstructorNotFoundException(string message) : base(message) { }
}