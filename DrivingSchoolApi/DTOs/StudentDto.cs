namespace DrivingSchoolApi.DTOs;

public sealed record StudentDto(
    Guid Id, 
    Guid SchoolId, 
    NameDto StudentName, 
    String EmailAddress, 
    String PhoneNumber, 
    List<TheoryLessonDto>? TheoryLessons = null, 
    List<DrivingLessonDto>? DrivingLessons = null);
 