namespace DrivingSchoolApi.DTOs;

public sealed record StudentDto(
    Guid Id, 
    Guid SchoolId, 
    NameDto StudentName, 
    String EmailAddress, 
    String PhoneNumber, 
    List<TheoryLessonDto>? TheoryLessons, 
    List<DrivingLessonDto>? DrivingLessons);
 