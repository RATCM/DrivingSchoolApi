namespace DrivingSchoolApi.Application.Exceptions.DrivingLesson;

public class DrivingLessonNotFoundException : ApplicationException
{
    public DrivingLessonNotFoundException(string message) : base(404, message)
    {
    }
}