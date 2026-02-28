namespace DrivingSchoolApi.DTOs;

public record TheoryLessonRegistry(
    Guid SchoolId,
    Guid InstructorId,
    DateTime LessonDateTime,
    MoneyDto Price,
    List<Guid> Students
    //TODO Instructor Signature
    );