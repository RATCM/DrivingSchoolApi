namespace DrivingSchoolApi.DTOs;

public record TheoryLessonRegistryDto(
    Guid SchoolId,
    Guid InstructorId,
    DateTime LessonDateTime,
    MoneyDto Price,
    List<Guid> StudentIds
    //TODO Instructor Signature
    );
    