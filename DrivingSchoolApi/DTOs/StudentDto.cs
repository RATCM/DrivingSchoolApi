namespace DrivingSchoolApi.DTOs;

public sealed record StudentDto(
    Guid Id, 
    Guid SchoolId, 
    NameDto StudentName, 
    string EmailAddress, 
    string PhoneNumber, 
    List<TheoryLessonDto>? TheoryLessons = null, 
    List<DrivingLessonDto>? DrivingLessons = null);
 