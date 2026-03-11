namespace DrivingSchoolApi.DTOs;

public sealed record StudentDto(
    Guid Id, 
    Guid SchoolId, 
    NameDto StudentName, 
    EmailDto EmailAddress, 
    PhoneNumberDto PhoneNumber, 
    List<TheoryLessonDto>? TheoryLessons, 
    List<DrivingLessonDto>? DrivingLessons);
 