namespace DrivingSchoolApi.Application.Exceptions.DrivingLesson;

public class DrivingLessonNotFoundException : NotFoundException
{
    public DrivingLessonNotFoundException(string message) : base(message) { }
}