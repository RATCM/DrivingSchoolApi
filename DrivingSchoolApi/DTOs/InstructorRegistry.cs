namespace DrivingSchoolApi.DTOs;

public sealed record InstructorRegistry(
    Guid SchoolId,
    NameDto Name,
    String EmailAddress,
    String PhoneNumber,
    string Password);
