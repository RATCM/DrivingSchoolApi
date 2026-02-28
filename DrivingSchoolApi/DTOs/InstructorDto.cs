namespace DrivingSchoolApi.DTOs;

public sealed record InstructorDto(
    Guid Id,
    Guid SchoolId,
    NameDto Name,
    EmailDto EmailAddress,
    PhoneNumberDto PhoneNumber,
    List<Guid>? TheoryLessonIDs,
    List<Guid>? DrivingLessonIds);
