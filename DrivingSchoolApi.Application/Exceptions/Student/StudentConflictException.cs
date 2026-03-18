namespace DrivingSchoolApi.Application.Exceptions.Student;

public class StudentConflictException(string message) : ApplicationException(400, message)
{
}