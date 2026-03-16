namespace DrivingSchoolApi.DTOs;

public sealed record InstructorRegistryDto(
    Guid SchoolId,
    NameDto Name,
    String EmailAddress,
    String PhoneNumber,
    string Password);
