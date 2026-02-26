namespace DrivingSchoolApi.DTOs;

public record TheoryLessonRegistry(
    Guid SchoolId,
    Guid InstructorId,
    DateTime LessonDateTime,
    string Price,
    List<Guid> Students);