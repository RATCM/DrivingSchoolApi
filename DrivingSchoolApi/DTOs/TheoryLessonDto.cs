namespace DrivingSchoolApi.DTOs;

public record TheoryLessonDto(
    Guid Id,
    Guid SchoolId,
    Guid InstructorId,
    DateTime LessonDateTime,
    string Price,
    List<Guid> Students);