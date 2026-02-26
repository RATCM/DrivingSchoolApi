namespace DrivingSchoolApi.DTOs;

public sealed record InstructorDto(
    Guid Id,
    Guid SchoolId, 
    string Name, 
    string EmailAddress, 
    string PhoneNumber,
    List<Guid> TheoryLessonIDs,
    List<Guid> DrivingLessonIds);