namespace DrivingSchoolApi.DTOs;

public sealed record InstructorRegistryDto(
    Guid SchoolId,
    NameDto Name,
    string Email,
    string PhoneNumber,
    string Password);
