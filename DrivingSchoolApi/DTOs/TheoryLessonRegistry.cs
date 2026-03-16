namespace DrivingSchoolApi.DTOs;

public record TheoryLessonRegistry(
    Guid SchoolId,
    Guid InstructorId,
    DateTime LessonDateTime,
    MoneyDto Price,
    List<Guid> StudentIds
    //TODO Instructor Signature
    );
    