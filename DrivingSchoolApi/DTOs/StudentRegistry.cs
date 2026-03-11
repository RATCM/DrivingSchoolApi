namespace DrivingSchoolApi.DTOs;

public sealed record StudentDtoRegistry(
    Guid SchoolId, 
    NameDto StudentName, 
    EmailDto EmailAddress, 
    PhoneNumberDto PhoneNumber,
    string Password);
    