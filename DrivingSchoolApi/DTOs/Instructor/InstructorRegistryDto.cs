using DrivingSchoolApi.DTOs.ValueObject;

namespace DrivingSchoolApi.DTOs.Instructor;

public sealed record InstructorRegistryDto(
    Guid SchoolId,
    NameDto Name,
    string Email,
    string PhoneNumber,
    string Password);
