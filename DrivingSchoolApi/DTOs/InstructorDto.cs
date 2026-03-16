namespace DrivingSchoolApi.DTOs;

public sealed record InstructorDto(
    Guid Id,
    Guid SchoolId,
    NameDto Name,
    String EmailAddress,
    String PhoneNumber,
    List<Guid>? TheoryLessonIDs,
    List<Guid>? DrivingLessonIds);
