namespace DrivingSchoolApi.Application.Exceptions.CompletedCourse;

public class CompletedCourseNotFoundException : NotFoundException
{
    public CompletedCourseNotFoundException(string message) : base(message) { }
}