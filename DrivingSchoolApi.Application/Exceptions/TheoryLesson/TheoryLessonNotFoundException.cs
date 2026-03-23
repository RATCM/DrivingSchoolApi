namespace DrivingSchoolApi.Application.Exceptions.TheoryLesson;

public class TheoryLessonNotFoundException : NotFoundException
{
    public TheoryLessonNotFoundException(string message) : base(message) { }
}