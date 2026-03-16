namespace DrivingSchoolApi.DTOs;

public sealed record StudentDtoRegistry(
    Guid SchoolId, 
    NameDto StudentName, 
    String EmailAddress, 
    String PhoneNumber,
    string Password);
    