namespace DrivingSchoolApi.DTOs;

public sealed record InstructorRegistry(
    Guid SchoolId,
    NameDto Name,
    EmailDto EmailAddress,
    PhoneNumberDto PhoneNumber,
    string Password);
