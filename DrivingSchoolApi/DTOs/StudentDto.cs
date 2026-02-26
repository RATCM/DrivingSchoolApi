namespace DrivingSchoolApi.DTOs;

public sealed record StudentDto(
    Guid Id, 
    Guid SchoolId, 
    string StudentName, 
    string EmailAddress, 
    string PhoneNumber, 
    List<Guid> TheoryLessonIDs, 
    List<Guid> DrivingLessonIds);
 